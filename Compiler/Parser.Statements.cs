using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Compiler {
	using AST;
	using AST.Statements;

	public partial class Parser {
		private void ParseFunctionDeclaration() {
			ParseFunction(true);
		}

		private IEnumerable<Statement> ParseStatementList(Statement parent) {
			for (
				var statement = ParseStatement(parent, false);
				statement != null;
				statement = ParseStatement(parent, false)
			) {
				yield return statement;
				if (_lookahead.Type == TokenType.RCurlyBrace)
					break;
				else if (_lookahead.Type == TokenType.Semicolon)
					ReadNextToken();
				else if (!_lookahead.IsAfterLineTerminator)
					ThrowUnmatchedToken(TokenType.Semicolon, _lookahead);
			}
		}

		private BlockStatement ParseBlockStatement(Statement parent) {
			var result = new BlockStatement(parent, _lookahead.StartPosition.LineNo);
			Match(TokenType.LCurlyBrace);
			foreach (var statement in ParseStatementList(result)) {
				result.AddStatement(statement);
			}
			Match(TokenType.RCurlyBrace);
			return (result);
		}

		private Statement ParseBreakStatement(Statement parent) {
			var startPosition = _lookahead.StartPosition;
			Match(TokenType.Break);
			string targetLabel;
			if (_lookahead.IsAfterLineTerminator)
				targetLabel = string.Empty;
			else if (_lookahead.Type == TokenType.Ident) {
				targetLabel = _lookahead.Value;
				ReadNextToken();
			}
			else {
				ThrowUnmatchedToken(TokenType.Ident, _lookahead);
				// Для того чтобы не ругался компилятор
				targetLabel = null;
			}
			var target = parent;
			while (target != null && !target.IsBreakTarget(targetLabel)) {
				target = target.Parent;
			}
			if (target == null)
				ThrowUnreachableLabel(startPosition, targetLabel);
			var result = new BreakStatement(parent, startPosition.LineNo, target);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseContinueStatement(Statement parent) {
			var startPosition = _lookahead.StartPosition;
			Match(TokenType.Continue);
			string targetLabel;
			if (_lookahead.IsAfterLineTerminator)
				targetLabel = string.Empty;
			else if (_lookahead.Type == TokenType.Ident) {
				targetLabel = _lookahead.Value;
				ReadNextToken();
			}
			else {
				ThrowUnmatchedToken(TokenType.Ident, _lookahead);
				// Для того чтобы не ругался компилятор
				targetLabel = null;
			}
			var target = parent;
			while (target != null && !target.IsContinueTarget(targetLabel)) {
				target = target.Parent;
			}
			if (target == null)
				ThrowUnreachableLabel(startPosition, targetLabel);
			var result = new ContinueStatement(parent, startPosition.LineNo, target);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseDoWhileStatement(Statement parent, ILabelSet labelSet) {
			var result = new DoWhileStatement(parent, _lookahead.StartPosition.LineNo, labelSet);
			Match(TokenType.Do);
			result.Statement = ParseStatement(result, true);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			return (result);
		}

		private Statement ParseEmptyStatement(Statement parent) {
			var result = new EmptyStatement(parent, _lookahead.StartPosition.LineNo);
			ReadNextToken();
			return (result);
		}

		private Statement ParseIfStatement(Statement parent) {
			var result = new IfStatement(parent, _lookahead.StartPosition.LineNo);
			Match(TokenType.If);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.ThenStatement = ParseStatement(result, true);
			if (_lookahead.Type == TokenType.Else) {
				ReadNextToken();
				result.ElseStatement = ParseStatement(result, true);
			}
			return (result);
		}

		private Expression ParseVariableDeclarationList() {
			var assignments = new List<Expression>();
			var hasMore = true;
			do {
				if (_lookahead.Type != TokenType.Ident)
					ThrowUnmatchedToken(TokenType.Ident, _lookahead);
				var variableName = _lookahead.Value;
				if (!_currentFunction.DeclaredVariables.Contains(variableName))
					_currentFunction.DeclaredVariables.Add(variableName);
				ReadNextToken();
				if (_lookahead.Type != TokenType.Assign) {
					assignments.Add(
						Expression.SimpleAssign(
							Expression.Ident(variableName), Expression.Undefined()
						)
					);
				}
				else {
					assignments.Add(
						Expression.SimpleAssign(
							Expression.Ident(variableName), ParseAssignmentExpression()
						)
					);
				}
				if (_lookahead.Type != TokenType.Comma)
					hasMore = false;
				else
					ReadNextToken();
			}
			while (hasMore);
			return (Expression.Sequence(assignments));
		}

		private Statement ParseForStatement(Statement parent, ILabelSet labelSet) {
			var startPosition = _lookahead.StartPosition;
			IterationStatement result = null;
			Match(TokenType.For);
			Match(TokenType.LParenthesis);
			var isVariableDeclaration = false;
			if (_lookahead.Type == TokenType.Var) {
				ReadNextToken();
				isVariableDeclaration = true;
			}
			if (_lookahead.Type == TokenType.Ident) {
				var variableName = _lookahead.Value;
				if (PeekNextToken().Type == TokenType.In) {
					MoveForwardLookahead();
					if (isVariableDeclaration) {
						if (!_currentFunction.DeclaredVariables.Contains(variableName))
							_currentFunction.DeclaredVariables.Add(variableName);
					}
					result = new ForInStatement(
						parent, startPosition.LineNo, variableName, ParseExpression(), labelSet
					);
				}
			}
			if (result == null) {
				Expression initialization = null;
				if (isVariableDeclaration) {
					initialization = ParseVariableDeclarationList();
				}
				else {
					if (_lookahead.Type != TokenType.Semicolon) {
						initialization = ParseExpression();
					}
				}
				Match(TokenType.Semicolon);
				Expression condition = null;
				if (_lookahead.Type != TokenType.Semicolon) {
					condition = ParseExpression();
				}
				Match(TokenType.Semicolon);
				Expression increment = null;
				if (_lookahead.Type != TokenType.RParenthesis) {
					increment = ParseExpression();
				}
				result = new ForStatement(
					parent, startPosition.LineNo, initialization, condition, increment, labelSet
				);
			}
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(result, true);
			return (result);
		}

		private Statement ParseReturnStatement(Statement parent) {
			var startPosition = _lookahead.StartPosition;
			Match(TokenType.Return);
			var expression = _lookahead.IsAfterLineTerminator ? Expression.Undefined() : ParseExpression();
			var result = new ReturnStatement(parent, startPosition.LineNo, expression);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseThrowStatement(Statement parent) {
			var startPosition = _lookahead.StartPosition;
			Match(TokenType.Throw);
			if (_lookahead.IsAfterLineTerminator)
				ThrowUnexpectedLineTerminator(_lookahead.StartPosition);
			return (new ThrowStatement(parent, startPosition.LineNo, ParseExpression()));
		}

		private Statement ParseTryStatement(Statement parent) {
			var result = new TryStatement(parent, _lookahead.StartPosition.LineNo);
			Match(TokenType.Try);
			result.TryBlock = ParseBlockStatement(result);
			var hasCatch = _lookahead.Type == TokenType.Catch;
			if (hasCatch) {
				ReadNextToken();
				Match(TokenType.LParenthesis);
				if (_lookahead.Type != TokenType.Ident)
					ThrowUnmatchedToken(TokenType.Ident, _lookahead);
				result.CatchBlockVariable = _lookahead.Value;
				ReadNextToken();
				Match(TokenType.LParenthesis);
				result.CatchBlock = ParseBlockStatement(result);
			}
			var hasFinally = _lookahead.Type == TokenType.Finally;
			if (hasFinally) {
				ReadNextToken();
				result.FinallyBlock = ParseBlockStatement(result);
			}
			if (!hasCatch && !hasFinally)
				ThrowExpectedCatchOrFinally(_lookahead.StartPosition);
			_currentFunction.RegisterTryBlock(result);
			return (result);
		}

		private IEnumerable<Statement> ParseCaseClauseStatementList(Statement parent) {
			var result = ParseStatementList(parent).ToList();
			if (result.Count == 0)
				return (Enumerable.Empty<Statement>());
			else
				return (result);
		}

		private CaseClause ParseCaseClause(Statement parent) {
			Match(TokenType.Case);
			var startPosition = _lookahead.StartPosition;
			var expression = ParseExpression();
			if (!expression.CanBeUsedInCaseClause)
				ThrowUnsupportedCaseClauseExpression(startPosition);
			Match(TokenType.Colon);
			return (new CaseClause(
				expression, ParseCaseClauseStatementList(parent)
			));
		}

		private Statement ParseSwitchStatement(Statement parent, ILabelSet labelSet) {
			var result = new SwitchStatement(parent, _lookahead.StartPosition.LineNo, labelSet);

			Match(TokenType.Switch);

			Match(TokenType.LParenthesis);
			result.Expression = ParseExpression();
			Match(TokenType.RParenthesis);

			Match(TokenType.LCurlyBrace);

			var beforeDefaultClauses = new List<CaseClause>();
			while (_lookahead.Type == TokenType.Case) {
				beforeDefaultClauses.Add(ParseCaseClause(result));
			}
			result.BeforeDefaultClauses = beforeDefaultClauses.Count == 0 ? Enumerable.Empty<CaseClause>() : beforeDefaultClauses;

			var hasDefaultClause = _lookahead.Type == TokenType.Default;
			if (hasDefaultClause) {
				ReadNextToken();
				Match(TokenType.Colon);
				result.DefaultClause = ParseCaseClauseStatementList(result);

				var afterDefaultClauses = new List<CaseClause>();
				while (_lookahead.Type == TokenType.Case) {
					afterDefaultClauses.Add(ParseCaseClause(result));
				}
				result.AfterDefaultClauses = afterDefaultClauses.Count == 0 ? Enumerable.Empty<CaseClause>() : afterDefaultClauses;
			}

			if (beforeDefaultClauses.Count == 0 && !hasDefaultClause)
				ThrowExpectedCaseClause(_lookahead.StartPosition);

			Match(TokenType.RCurlyBrace);

			return (result);
		}

		private Statement ParseVariableDeclarationStatement(Statement parent) {
			var startPosition = _lookahead.StartPosition;
			Match(TokenType.Var);
			return (new ExpressionStatement(parent, startPosition.LineNo, ParseVariableDeclarationList()));
		}

		private Statement ParseWhileStatement(Statement parent, ILabelSet labelSet) {
			var result = new WhileStatement(parent, _lookahead.StartPosition.LineNo, labelSet);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(result, true);
			return (result);
		}

		private Statement ParseExpressionStatement(Statement parent) {
			return (new ExpressionStatement(
				parent, _lookahead.StartPosition.LineNo, ParseExpression()
			));
		}

		private Statement ParseStatement(Statement parent, bool isRequired) {
			Contract.Requires(parent != null);

			var labelSet = Statement.EmptyLabelSet;

			// Вычислить набор меток текущего оператора
			while (_lookahead.Type == TokenType.Ident) {
				var possibleLabel = _lookahead.Value;
				if (PeekNextToken().Type != TokenType.Colon)
					break;
				if (labelSet.Contains(possibleLabel))
					ThrowDuplicatedLabel(_lookahead.StartPosition, possibleLabel);
				labelSet = labelSet.UnionWith(possibleLabel);
				MoveForwardLookahead();
			}

			// Разобрать оператор
			switch (_lookahead.Type) {
				case TokenType.Unknown:
				case TokenType.Case:
				case TokenType.Default:
				case TokenType.RCurlyBrace:
					if (isRequired)
						ThrowExpectedStatement(_lookahead.StartPosition);
					return (null);
				case TokenType.LCurlyBrace:
					return (ParseBlockStatement(parent));
				case TokenType.Break:
					return (ParseBreakStatement(parent));
				case TokenType.Continue:
					return (ParseContinueStatement(parent));
				case TokenType.Do:
					return (ParseDoWhileStatement(parent, labelSet));
				case TokenType.Semicolon:
					return (ParseEmptyStatement(parent));
				case TokenType.If:
					return (ParseIfStatement(parent));
				case TokenType.For:
					return (ParseForStatement(parent, labelSet));
				case TokenType.Return:
					return (ParseReturnStatement(parent));
				case TokenType.Throw:
					return (ParseThrowStatement(parent));
				case TokenType.Try:
					return (ParseTryStatement(parent));
				case TokenType.Switch:
					return (ParseSwitchStatement(parent, labelSet));
				case TokenType.Var:
					return (ParseVariableDeclarationStatement(parent));
				case TokenType.While:
					return (ParseWhileStatement(parent, labelSet));
				default:
					return (ParseExpressionStatement(parent));
			}
		}

		private void ParseFunctionBody(FunctionBody functionBody) {
			Contract.Requires(functionBody != null);
			for (; ; ) {
				if (_lookahead.Type == TokenType.Function) {
					// ReadNextToken вызовет ParseFunctionDeclaration
					ParseFunctionDeclaration();
				}
				else {
					var statement = ParseStatement(functionBody, false);
					if (statement == null)
						break;
					functionBody.AddStatement(statement);
					if (_lookahead.Type == TokenType.Unknown)
						break;
					else if (_lookahead.Type == TokenType.Semicolon)
						ReadNextToken();
					else if (!_lookahead.IsAfterLineTerminator)
						ThrowUnmatchedToken(TokenType.Semicolon, _lookahead);
				}
			}
		}
	}
}
