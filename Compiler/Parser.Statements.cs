using System;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	using YaJS.Compiler.AST;
	using YaJS.Compiler.AST.Statements;

	public partial class Parser {

		private void ParseFunctionDeclaration() {
			Match(TokenType.Function);
			throw new NotImplementedException();
		}

		private Statement ParseBlockStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseBreakStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseDoWhileStatement(Statement parent, ILabelSet labelSet) {
			throw new NotImplementedException();
		}

		private Statement ParseEmptyStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseIfStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseForStatement(Statement parent, ILabelSet labelSet) {
			throw new NotImplementedException();
		}

		private Statement ParseReturnStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseThrowStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseTryStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseSwitchStatement(Statement parent, ILabelSet labelSet) {
			throw new NotImplementedException();
		}

		private Statement ParseVariableDeclarationStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseWhileStatement(Statement parent, ILabelSet labelSet) {
			throw new NotImplementedException();
		}

		private Statement ParseExpressionStatement(Statement parent) {
			throw new NotImplementedException();
		}

		private Statement ParseStatement(Statement parent) {
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
					return (null);
				case TokenType.LCurlyBrace:
					return (ParseBlockStatement(parent));
				case TokenType.Break:
					return (ParseBreakStatement(parent));
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
			for (;;) {
				if (_lookahead.Type == TokenType.Function) {
					// ReadNextToken вызовет ParseFunctionDeclaration
					ParseFunctionDeclaration();
				}
				else {
					var statement = ParseStatement(root);
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
