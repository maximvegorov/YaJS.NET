using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Statements;

namespace YaJS.Compiler {
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
				if (Lookahead.Type == TokenType.RCurlyBrace)
					break;
				if (Lookahead.Type == TokenType.Semicolon)
					ReadNextToken();
				else if (!Lookahead.IsAfterLineTerminator)
					ThrowUnmatchedToken(TokenType.Semicolon, Lookahead);
			}
		}

		private BlockStatement ParseBlockStatement(Statement parent) {
			var result = new BlockStatement(parent, Lookahead.StartPosition.LineNo);
			Match(TokenType.LCurlyBrace);
			foreach (var statement in ParseStatementList(result))
				result.AddStatement(statement);
			Match(TokenType.RCurlyBrace);
			return (result);
		}

		private Statement ParseBreakStatement(Statement parent) {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Break);
			string targetLabel;
			if (Lookahead.IsAfterLineTerminator)
				targetLabel = string.Empty;
			else if (Lookahead.Type == TokenType.Ident) {
				targetLabel = Lookahead.Value;
				ReadNextToken();
			}
			else {
				ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				// Для того чтобы не ругался компилятор
				targetLabel = null;
			}
			var target = parent;
			while (target != null && !target.IsBreakTarget(targetLabel))
				target = target.Parent;
			if (target == null)
				ThrowUnreachableLabel(startPosition, targetLabel);
			var result = new BreakStatement(parent, startPosition.LineNo, target);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseContinueStatement(Statement parent) {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Continue);
			string targetLabel;
			if (Lookahead.IsAfterLineTerminator)
				targetLabel = string.Empty;
			else if (Lookahead.Type == TokenType.Ident) {
				targetLabel = Lookahead.Value;
				ReadNextToken();
			}
			else {
				ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				// Для того чтобы не ругался компилятор
				targetLabel = null;
			}
			var target = parent;
			while (target != null && !target.IsContinueTarget(targetLabel))
				target = target.Parent;
			if (target == null)
				ThrowUnreachableLabel(startPosition, targetLabel);
			var result = new ContinueStatement(parent, startPosition.LineNo, target);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseDoWhileStatement(Statement parent, ILabelSet labelSet) {
			var result = new DoWhileStatement(parent, Lookahead.StartPosition.LineNo, labelSet);
			Match(TokenType.Do);
			result.Statement = ParseStatement(result, true);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			return (result);
		}

		private Statement ParseEmptyStatement(Statement parent) {
			var result = new EmptyStatement(parent, Lookahead.StartPosition.LineNo);
			ReadNextToken();
			return (result);
		}

		private Statement ParseIfStatement(Statement parent) {
			var result = new IfStatement(parent, Lookahead.StartPosition.LineNo);
			Match(TokenType.If);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.ThenStatement = ParseStatement(result, true);
			if (Lookahead.Type == TokenType.Else) {
				ReadNextToken();
				result.ElseStatement = ParseStatement(result, true);
			}
			return (result);
		}

		private Expression ParseVariableDeclarationList() {
			var assignments = new List<Expression>();
			var hasMore = true;
			do {
				if (Lookahead.Type != TokenType.Ident)
					ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				var variableName = Lookahead.Value;
				if (!_currentFunction.DeclaredVariables.Contains(variableName))
					_currentFunction.DeclaredVariables.Add(variableName);
				ReadNextToken();
				if (Lookahead.Type != TokenType.Assign) {
					assignments.Add(
						Expression.SimpleAssign(
							Expression.Ident(variableName),
							Expression.Undefined()
							)
						);
				}
				else {
					assignments.Add(
						Expression.SimpleAssign(
							Expression.Ident(variableName),
							ParseAssignmentExpression()
							)
						);
				}
				if (Lookahead.Type != TokenType.Comma)
					hasMore = false;
				else
					ReadNextToken();
			} while (hasMore);
			return (Expression.Sequence(assignments));
		}

		private Statement ParseForStatement(Statement parent, ILabelSet labelSet) {
			var startPosition = Lookahead.StartPosition;
			IterationStatement result = null;
			Match(TokenType.For);
			Match(TokenType.LParenthesis);
			var isVariableDeclaration = false;
			if (Lookahead.Type == TokenType.Var) {
				ReadNextToken();
				isVariableDeclaration = true;
			}
			if (Lookahead.Type == TokenType.Ident) {
				var variableName = Lookahead.Value;
				if (PeekNextToken().Type == TokenType.In) {
					MoveForwardLookahead();
					if (isVariableDeclaration) {
						if (!_currentFunction.DeclaredVariables.Contains(variableName))
							_currentFunction.DeclaredVariables.Add(variableName);
					}
					result = new ForInStatement(
						parent,
						startPosition.LineNo,
						variableName,
						ParseExpression(),
						labelSet
						);
				}
			}
			if (result == null) {
				Expression initialization = null;
				if (isVariableDeclaration)
					initialization = ParseVariableDeclarationList();
				else {
					if (Lookahead.Type != TokenType.Semicolon)
						initialization = ParseExpression();
				}
				Match(TokenType.Semicolon);
				Expression condition = null;
				if (Lookahead.Type != TokenType.Semicolon)
					condition = ParseExpression();
				Match(TokenType.Semicolon);
				Expression increment = null;
				if (Lookahead.Type != TokenType.RParenthesis)
					increment = ParseExpression();
				result = new ForStatement(
					parent,
					startPosition.LineNo,
					initialization,
					condition,
					increment,
					labelSet
					);
			}
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(result, true);
			return (result);
		}

		private Statement ParseReturnStatement(Statement parent) {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Return);
			var expression = Lookahead.IsAfterLineTerminator ? Expression.Undefined() : ParseExpression();
			var result = new ReturnStatement(parent, startPosition.LineNo, expression);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseThrowStatement(Statement parent) {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Throw);
			if (Lookahead.IsAfterLineTerminator)
				ThrowUnexpectedLineTerminator(Lookahead.StartPosition);
			return (new ThrowStatement(parent, startPosition.LineNo, ParseExpression()));
		}

		private Statement ParseTryStatement(Statement parent) {
			var result = new TryStatement(parent, Lookahead.StartPosition.LineNo);
			Match(TokenType.Try);
			result.TryBlock = ParseBlockStatement(result);
			var hasCatch = Lookahead.Type == TokenType.Catch;
			if (hasCatch) {
				ReadNextToken();
				Match(TokenType.LParenthesis);
				if (Lookahead.Type != TokenType.Ident)
					ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				result.CatchBlockVariable = Lookahead.Value;
				ReadNextToken();
				Match(TokenType.LParenthesis);
				result.CatchBlock = ParseBlockStatement(result);
			}
			var hasFinally = Lookahead.Type == TokenType.Finally;
			if (hasFinally) {
				ReadNextToken();
				result.FinallyBlock = ParseBlockStatement(result);
			}
			if (!hasCatch && !hasFinally)
				ThrowExpectedCatchOrFinally(Lookahead.StartPosition);
			_currentFunction.RegisterTryBlock(result);
			return (result);
		}

		private IEnumerable<Statement> ParseCaseClauseStatementList(Statement parent) {
			var result = ParseStatementList(parent).ToList();
			if (result.Count == 0)
				return (Enumerable.Empty<Statement>());
			return (result);
		}

		private CaseClause ParseCaseClause(Statement parent) {
			Match(TokenType.Case);
			var startPosition = Lookahead.StartPosition;
			var expression = ParseExpression();
			if (!expression.CanBeUsedInCaseClause)
				ThrowUnsupportedCaseClauseExpression(startPosition);
			Match(TokenType.Colon);
			return (new CaseClause(
				expression,
				ParseCaseClauseStatementList(parent)
				));
		}

		private Statement ParseSwitchStatement(Statement parent, ILabelSet labelSet) {
			var result = new SwitchStatement(parent, Lookahead.StartPosition.LineNo, labelSet);

			Match(TokenType.Switch);

			Match(TokenType.LParenthesis);
			result.Expression = ParseExpression();
			Match(TokenType.RParenthesis);

			Match(TokenType.LCurlyBrace);

			var beforeDefaultClauses = new List<CaseClause>();
			while (Lookahead.Type == TokenType.Case)
				beforeDefaultClauses.Add(ParseCaseClause(result));
			result.BeforeDefaultClauses = beforeDefaultClauses.Count == 0 ? Enumerable.Empty<CaseClause>() : beforeDefaultClauses;

			var hasDefaultClause = Lookahead.Type == TokenType.Default;
			if (hasDefaultClause) {
				ReadNextToken();
				Match(TokenType.Colon);
				result.DefaultClause = ParseCaseClauseStatementList(result);

				var afterDefaultClauses = new List<CaseClause>();
				while (Lookahead.Type == TokenType.Case)
					afterDefaultClauses.Add(ParseCaseClause(result));
				result.AfterDefaultClauses = afterDefaultClauses.Count == 0 ? Enumerable.Empty<CaseClause>() : afterDefaultClauses;
			}

			if (beforeDefaultClauses.Count == 0 && !hasDefaultClause)
				ThrowExpectedCaseClause(Lookahead.StartPosition);

			Match(TokenType.RCurlyBrace);

			return (result);
		}

		private Statement ParseVariableDeclarationStatement(Statement parent) {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Var);
			return (new ExpressionStatement(parent, startPosition.LineNo, ParseVariableDeclarationList()));
		}

		private Statement ParseWhileStatement(Statement parent, ILabelSet labelSet) {
			var result = new WhileStatement(parent, Lookahead.StartPosition.LineNo, labelSet);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(result, true);
			return (result);
		}

		private Statement ParseExpressionStatement(Statement parent) {
			return (new ExpressionStatement(
				parent,
				Lookahead.StartPosition.LineNo,
				ParseExpression()
				));
		}

		private Statement ParseStatement(Statement parent, bool isRequired) {
			Contract.Requires(parent != null);

			var labelSet = Statement.EmptyLabelSet;

			// Вычислить набор меток текущего оператора
			while (Lookahead.Type == TokenType.Ident) {
				var possibleLabel = Lookahead.Value;
				if (PeekNextToken().Type != TokenType.Colon)
					break;
				if (labelSet.Contains(possibleLabel))
					ThrowDuplicatedLabel(Lookahead.StartPosition, possibleLabel);
				labelSet = labelSet.UnionWith(possibleLabel);
				MoveForwardLookahead();
			}

			// Разобрать оператор
			switch (Lookahead.Type) {
				case TokenType.Unknown:
				case TokenType.Case:
				case TokenType.Default:
				case TokenType.RCurlyBrace:
					if (isRequired)
						ThrowExpectedStatement(Lookahead.StartPosition);
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
			for (;;) {
				if (Lookahead.Type == TokenType.Function) {
					// ReadNextToken вызовет ParseFunctionDeclaration
					ParseFunctionDeclaration();
				}
				else {
					var statement = ParseStatement(functionBody, false);
					if (statement == null)
						break;
					functionBody.AddStatement(statement);
					if (Lookahead.Type == TokenType.Unknown)
						break;
					if (Lookahead.Type == TokenType.Semicolon)
						ReadNextToken();
					else if (!Lookahead.IsAfterLineTerminator)
						ThrowUnmatchedToken(TokenType.Semicolon, Lookahead);
				}
			}
		}
	}
}
