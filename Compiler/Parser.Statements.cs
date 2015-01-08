using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	using YaJS.Compiler.AST;
	using YaJS.Compiler.AST.Statements;

	public partial class Parser {
		private void ParseFunctionDeclaration() {
			ParseFunction(true);
		}

		private BlockStatement ParseBlockStatement(Statement parent) {
			var result = new BlockStatement(parent);
			Match(TokenType.LCurlyBrace);
			for (
				var statement = ParseStatement(result, false);
				statement != null;
				statement = ParseStatement(result, false)
			) {
				result.AddStatement(statement);
				if (_lookahead.Type == TokenType.RCurlyBrace)
					break;
				else if (_lookahead.Type == TokenType.Semicolon)
					ReadNextToken();
				else if (!_lookahead.IsAfterLineTerminator)
					ThrowUnmatchedToken(TokenType.Semicolon, _lookahead);
			}
			Match(TokenType.RCurlyBrace);
			return (result);
		}

		private Statement ParseBreakStatement(Statement parent) {
			var breakPosition = _lookahead.StartPosition;
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
				ThrowUnreachableLabel(breakPosition, targetLabel);
			var result = new BreakStatement(parent, target);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseContinueStatement(Statement parent) {
			var continuePosition = _lookahead.StartPosition;
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
				ThrowUnreachableLabel(continuePosition, targetLabel);
			var result = new ContinueStatement(parent, target);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseDoWhileStatement(Statement parent, ILabelSet labelSet) {
			var result = new DoWhileStatement(parent, labelSet);
			Match(TokenType.Do);
			result.Statement = ParseStatement(result, true);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			return (result);
		}

		private Statement ParseEmptyStatement(Statement parent) {
			ReadNextToken();
			return (new EmptyStatement(parent));
		}

		private Statement ParseIfStatement(Statement parent) {
			var result = new IfStatement(parent);
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
					result = new ForInStatement(parent, variableName, ParseExpression(), labelSet);
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
				result = new ForStatement(parent, initialization, condition, increment, labelSet);
			}
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(result, true);
			return (result);
		}

		private Statement ParseReturnStatement(Statement parent) {
			if (!parent.CanContainReturn())
				ThrowInvalidStatement(_lookahead.StartPosition);
			Match(TokenType.Return);
			Expression expression;
			if (_lookahead.IsAfterLineTerminator)
				expression = Expression.Undefined();
			else
				expression = ParseExpression();
			var result = new ReturnStatement(parent, expression);
			result.RegisterAsExitPoint();
			return (result);
		}

		private Statement ParseThrowStatement(Statement parent) {
			Match(TokenType.Throw);
			if (_lookahead.IsAfterLineTerminator)
				ThrowUnexpectedLineTerminator(_lookahead.StartPosition);
			return (new ThrowStatement(parent, ParseExpression()));
		}

		private Statement ParseTryStatement(Statement parent) {
			Match(TokenType.Try);
			var result = new TryStatement(parent);
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

		private Statement ParseSwitchStatement(Statement parent, ILabelSet labelSet) {
			throw new NotImplementedException();
		}

		private Statement ParseVariableDeclarationStatement(Statement parent) {
			Match(TokenType.Var);
			return (new ExpressionStatement(parent, ParseVariableDeclarationList()));
		}

		private Statement ParseWhileStatement(Statement parent, ILabelSet labelSet) {
			var result = new WhileStatement(parent, labelSet);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(result, true);
			return (result);
		}

		private Statement ParseExpressionStatement(Statement parent) {
			return (new ExpressionStatement(parent, ParseExpression()));
		}

		private Statement ParseStatement(Statement parent, bool isRequired) {
			Contract.Requires(parent != null);

			ILabelSet labelSet = Statement.EmptyLabelSet;

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

		private void ParseStatementList(RootStatement root) {
			Contract.Requires(root != null);
			for (; ; ) {
				if (_lookahead.Type == TokenType.Function) {
					// ReadNextToken вызовет ParseFunctionDeclaration
					ParseFunctionDeclaration();
				}
				else {
					var statement = ParseStatement(root, false);
					if (statement == null)
						break;
					root.AddStatement(statement);
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
