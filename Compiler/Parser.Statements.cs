using System.Collections.Generic;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Statements;

namespace YaJS.Compiler {
	public partial class Parser {
		private void ParseFunctionDeclaration() {
			ParseFunction(true);
		}

		private IEnumerable<Statement> ParseStatementList() {
			for (var statement = ParseStatement(false); statement != null; statement = ParseStatement(false)) {
				yield return statement;
				if (Lookahead.Type == TokenType.RCurlyBrace)
					break;
				if (Lookahead.Type == TokenType.Semicolon)
					ReadNextToken();
				else if (!Lookahead.IsAfterLineTerminator)
					Errors.ThrowUnmatchedToken(TokenType.Semicolon, Lookahead);
			}
		}

		private BlockStatement ParseBlockStatement() {
			var result = new BlockStatement(Lookahead.StartPosition.LineNo);
			Match(TokenType.LCurlyBrace);
			foreach (var statement in ParseStatementList())
				result.Add(statement);
			Match(TokenType.RCurlyBrace);
			return (result);
		}

		private Statement ParseBreakStatement() {
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
				Errors.ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				// Для того чтобы не ругался компилятор
				targetLabel = null;
			}
			return (new BreakStatement(startPosition.LineNo, targetLabel));
		}

		private Statement ParseContinueStatement() {
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
				Errors.ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				// Для того чтобы не ругался компилятор
				targetLabel = null;
			}
			return (new ContinueStatement(startPosition.LineNo, targetLabel));
		}

		private Statement ParseDoWhileStatement(ILabelSet labelSet) {
			var result = new DoWhileStatement(Lookahead.StartPosition.LineNo, labelSet);
			Match(TokenType.Do);
			result.Statement = ParseStatement(true);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			return (result);
		}

		private Statement ParseEmptyStatement() {
			var result = new EmptyStatement(Lookahead.StartPosition.LineNo);
			ReadNextToken();
			return (result);
		}

		private Statement ParseIfStatement() {
			var result = new IfStatement(Lookahead.StartPosition.LineNo);
			Match(TokenType.If);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.ThenStatement = ParseStatement(true);
			if (Lookahead.Type == TokenType.Else) {
				ReadNextToken();
				result.ElseStatement = ParseStatement(true);
			}
			return (result);
		}

		private Expression ParseVariableDeclarationList() {
			var assignments = new List<Expression>();
			var hasMore = true;
			do {
				if (Lookahead.Type != TokenType.Ident)
					Errors.ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				var variableName = Lookahead.Value;
				if (!_currentFunction.DeclaredVariables.Contains(variableName))
					_currentFunction.DeclaredVariables.Add(variableName);
				ReadNextToken();
				if (Lookahead.Type != TokenType.Assign)
					assignments.Add(Expression.SimpleAssign(Expression.Ident(variableName), Expression.Undefined()));
				else {
					ReadNextToken();
					assignments.Add(Expression.SimpleAssign(Expression.Ident(variableName), ParseAssignmentExpression()));
				}
				if (Lookahead.Type != TokenType.Comma)
					hasMore = false;
				else
					ReadNextToken();
			} while (hasMore);
			return (Expression.Sequence(assignments));
		}

		private Statement ParseForStatement(ILabelSet labelSet) {
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
					result = new ForInStatement(startPosition.LineNo, variableName, ParseExpression(), labelSet);
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
				result = new ForStatement(startPosition.LineNo, initialization, condition, increment, labelSet);
			}
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(true);
			return (result);
		}

		private Statement ParseReturnStatement() {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Return);
			var expression = Lookahead.IsAfterLineTerminator ? Expression.Undefined() : ParseExpression();
			return (new ReturnStatement(startPosition.LineNo, expression));
		}

		private Statement ParseThrowStatement() {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Throw);
			if (Lookahead.IsAfterLineTerminator)
				Errors.ThrowUnexpectedLineTerminator(Lookahead.StartPosition);
			return (new ThrowStatement(startPosition.LineNo, ParseExpression()));
		}

		private TryBlockStatement ParseTryBlockStatement() {
			var result = new TryBlockStatement(Lookahead.StartPosition.LineNo);
			Match(TokenType.LCurlyBrace);
			foreach (var statement in ParseStatementList())
				result.Add(statement);
			Match(TokenType.RCurlyBrace);
			return (result);
		}

		private Statement ParseTryStatement() {
			var result = new TryStatement(Lookahead.StartPosition.LineNo);
			Match(TokenType.Try);
			result.TryBlock = ParseTryBlockStatement();
			var hasCatch = Lookahead.Type == TokenType.Catch;
			if (hasCatch) {
				ReadNextToken();
				Match(TokenType.LParenthesis);
				if (Lookahead.Type != TokenType.Ident)
					Errors.ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				result.CatchBlockVariable = Lookahead.Value;
				ReadNextToken();
				Match(TokenType.LParenthesis);
				result.CatchBlock = ParseBlockStatement();
			}
			var hasFinally = Lookahead.Type == TokenType.Finally;
			if (hasFinally) {
				ReadNextToken();
				result.FinallyBlock = ParseBlockStatement();
			}
			if (!hasCatch && !hasFinally)
				Errors.ThrowExpectedCatchOrFinally(Lookahead.StartPosition);
			return (result);
		}

		private StatementListStatement ParseStatementListStatement() {
			var result = new StatementListStatement(Lookahead.StartPosition.LineNo);
			foreach (var statement in ParseStatementList())
				result.Add(statement);
			return (result);
		}

		private CaseClauseStatement ParseCaseClauseStatement() {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Case);
			var expression = ParseExpression();
			if (!expression.CanBeUsedInCaseClause)
				Errors.ThrowUnsupportedCaseClauseExpression(startPosition);
			Match(TokenType.Colon);
			return (new CaseClauseStatement(startPosition.LineNo, expression) {Statements = ParseStatementListStatement()});
		}

		private CaseClauseBlockStatement ParseCaseClauseBlockStatement() {
			var result = new CaseClauseBlockStatement(Lookahead.StartPosition.LineNo);
			while (Lookahead.Type == TokenType.Case)
				result.Add(ParseCaseClauseStatement());
			return (result);
		}

		private Statement ParseSwitchStatement(ILabelSet labelSet) {
			var result = new SwitchStatement(Lookahead.StartPosition.LineNo, labelSet);

			Match(TokenType.Switch);

			Match(TokenType.LParenthesis);
			result.Expression = ParseExpression();
			Match(TokenType.RParenthesis);

			Match(TokenType.LCurlyBrace);

			result.BeforeDefault = ParseCaseClauseBlockStatement();

			if (Lookahead.Type == TokenType.Default) {
				ReadNextToken();
				Match(TokenType.Colon);
				result.DefaultClause = ParseStatementListStatement();

				result.AfterDefault = ParseCaseClauseBlockStatement();
			}

			if (result.BeforeDefault.IsEmpty || (result.AfterDefault != null && result.AfterDefault.IsEmpty))
				Errors.ThrowExpectedCaseClause(Lookahead.StartPosition);

			Match(TokenType.RCurlyBrace);

			return (result);
		}

		private Statement ParseVariableDeclarationStatement() {
			var startPosition = Lookahead.StartPosition;
			Match(TokenType.Var);
			return (new ExpressionStatement(startPosition.LineNo, ParseVariableDeclarationList()));
		}

		private Statement ParseWhileStatement(ILabelSet labelSet) {
			var result = new WhileStatement(Lookahead.StartPosition.LineNo, labelSet);
			Match(TokenType.While);
			Match(TokenType.LParenthesis);
			result.Condition = ParseExpression();
			Match(TokenType.RParenthesis);
			result.Statement = ParseStatement(true);
			return (result);
		}

		private Statement ParseExpressionStatement() {
			return (new ExpressionStatement(Lookahead.StartPosition.LineNo, ParseExpression()));
		}

		private Statement ParseStatement(bool isRequired) {
			var labelSet = Statement.EmptyLabelSet;

			// Вычислить набор меток текущего оператора
			while (Lookahead.Type == TokenType.Ident) {
				var possibleLabel = Lookahead.Value;
				if (PeekNextToken().Type != TokenType.Colon)
					break;
				if (labelSet.Contains(possibleLabel))
					Errors.ThrowDuplicatedLabel(Lookahead.StartPosition, possibleLabel);
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
						Errors.ThrowExpectedStatement(Lookahead.StartPosition);
					return (null);
				case TokenType.LCurlyBrace:
					return (ParseBlockStatement());
				case TokenType.Break:
					return (ParseBreakStatement());
				case TokenType.Continue:
					return (ParseContinueStatement());
				case TokenType.Do:
					return (ParseDoWhileStatement(labelSet));
				case TokenType.Semicolon:
					return (ParseEmptyStatement());
				case TokenType.If:
					return (ParseIfStatement());
				case TokenType.For:
					return (ParseForStatement(labelSet));
				case TokenType.Return:
					return (ParseReturnStatement());
				case TokenType.Throw:
					return (ParseThrowStatement());
				case TokenType.Try:
					return (ParseTryStatement());
				case TokenType.Switch:
					return (ParseSwitchStatement(labelSet));
				case TokenType.Var:
					return (ParseVariableDeclarationStatement());
				case TokenType.While:
					return (ParseWhileStatement(labelSet));
				default:
					return (ParseExpressionStatement());
			}
		}

		private FunctionBodyStatement ParseFunctionBody() {
			var result = new FunctionBodyStatement();
			for (;;) {
				if (Lookahead.Type == TokenType.Function) {
					// ReadNextToken вызовет ParseFunctionDeclaration
					ParseFunctionDeclaration();
				}
				else {
					var statement = ParseStatement(false);
					if (statement == null)
						break;
					result.Add(statement);
					if (Lookahead.Type == TokenType.Unknown)
						break;
					if (Lookahead.Type == TokenType.Semicolon)
						ReadNextToken();
					else if (!Lookahead.IsAfterLineTerminator)
						Errors.ThrowUnmatchedToken(TokenType.Semicolon, Lookahead);
				}
			}
			return (result);
		}
	}
}
