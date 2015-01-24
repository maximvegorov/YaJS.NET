using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;

namespace YaJS.Compiler {
	/// <summary>
	/// Синтаксический анализатор
	/// </summary>
	public partial class Parser {
		private readonly LinkedList<Token> _peekTokens;
		private readonly Tokenizer _tokenizer;
		private FunctionBuilder _currentFunction;

		public Parser(Tokenizer tokenizer) {
			Contract.Requires<ArgumentNullException>(tokenizer != null, "tokenizer");
			_tokenizer = tokenizer;
			_peekTokens = new LinkedList<Token>();
			ReadNextToken();
		}

		private void ReadNextToken() {
			if (_peekTokens.Count == 0)
				Lookahead = _tokenizer.ReadToken();
			else {
				Lookahead = _peekTokens.First.Value;
				_peekTokens.RemoveFirst();
			}
		}

		private Token PeekNextToken() {
			// Если подглядываем первый раз, то необходимо сохранить _lookahead иначе _tokenizer перезатрет
			if (_peekTokens.Count == 0)
				Lookahead = Lookahead.Clone();
			else
				_peekTokens.Last.Value = _peekTokens.Last.Value.Clone();
			var result = _tokenizer.ReadToken();
			_peekTokens.AddLast(result);
			return (result);
		}

		private void MoveForwardLookahead() {
			Contract.Requires(_peekTokens.Count > 0);
			_peekTokens.Clear();
			Lookahead = _tokenizer.ReadToken();
		}

		private void Match(TokenType expectedTokenType) {
			if (Lookahead.Type != expectedTokenType)
				Errors.ThrowUnmatchedToken(expectedTokenType, Lookahead);
			ReadNextToken();
		}

		private Function ParseFunction(bool isDeclaration) {
			Contract.Requires(_currentFunction != null);
			Contract.Ensures(_currentFunction != null);

			var startPosition = Lookahead.StartPosition;

			Match(TokenType.Function);

			string name = null;
			if (Lookahead.Type == TokenType.Ident) {
				name = Lookahead.Value;
				if (_currentFunction.Outer.NestedFunctions.Contains(name))
					Errors.ThrowFunctionAlreadyDeclared(startPosition, name);
				ReadNextToken();
			}

			var parameterNames = new KeyedVariableCollection();
			Match(TokenType.LParenthesis);
			if (Lookahead.Type != TokenType.RParenthesis) {
				if (Lookahead.Type != TokenType.Ident)
					Errors.ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				parameterNames.Add(Lookahead.Value);
				ReadNextToken();
				while (Lookahead.Type == TokenType.Comma) {
					ReadNextToken();
					if (Lookahead.Type != TokenType.Ident)
						Errors.ThrowUnmatchedToken(TokenType.Ident, Lookahead);
					var parameterName = Lookahead.Value;
					if (parameterNames.Contains(parameterName))
						Errors.ThrowParameterAlreadyDeclared(Lookahead.StartPosition, parameterName);
					parameterNames.Add(parameterName);
					ReadNextToken();
				}
			}
			Match(TokenType.RParenthesis);

			_currentFunction = new FunctionBuilder(
				_currentFunction,
				name,
				startPosition.LineNo,
				parameterNames,
				isDeclaration
				);
			Match(TokenType.LCurlyBrace);
			_currentFunction.FunctionBody = ParseFunctionBody();
			Match(TokenType.RCurlyBrace);

			var result = _currentFunction.ToFunction();
			_currentFunction = _currentFunction.Outer;
			_currentFunction.NestedFunctions.Add(result);
			return (result);
		}

		private static IKeyedVariableCollection ToVariableCollection(IEnumerable<string> parameterNames) {
			var result = new KeyedVariableCollection();
			foreach (var parameterName in parameterNames) {
				if (result.Contains(parameterName))
					Errors.ThrowParameterAlreadyDeclared(parameterName);
				result.Add(parameterName);
			}
			return (result);
		}

		public Function ParseFunction(string functionName, IEnumerable<string> parameterNames) {
			Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(functionName), "functionName");
			Contract.Requires<ArgumentNullException>(parameterNames != null, "parameterNames");
			// ReSharper disable once UseObjectOrCollectionInitializer
			_currentFunction = new FunctionBuilder(
				null,
				functionName,
				1,
				ToVariableCollection(parameterNames),
				false
				);
			_currentFunction.FunctionBody = ParseFunctionBody();
			Contract.Assert(Lookahead.Type == TokenType.Unknown);
			return (_currentFunction.ToFunction());
		}

		public Token Lookahead { get; private set; }
	}
}
