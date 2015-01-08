using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace YaJS.Compiler {
	using YaJS.Compiler.AST;
	using YaJS.Compiler.AST.Statements;
	using YaJS.Compiler.Exceptions;

	/// <summary>
	/// Синтаксический анализатор
	/// </summary>
	public partial class Parser {
		private Tokenizer _tokenizer;
		private LinkedList<Token> _peekTokens;
		private Token _lookahead;
		private FunctionContext _currentFunction;

		public Parser(Tokenizer tokenizer) {
			Contract.Requires<ArgumentNullException>(tokenizer != null, "tokenizer");
			_tokenizer = tokenizer;
			_peekTokens = new LinkedList<Token>();
			ReadNextToken();
		}

		private void ReadNextToken() {
			if (_peekTokens.Count == 0)
				_lookahead = _tokenizer.ReadToken();
			else {
				_lookahead = _peekTokens.First.Value;
				_peekTokens.RemoveFirst();
			}
		}

		private Token PeekNextToken() {
			// Если подглядываем первый раз, то необходимо сохранить _lookahead иначе _tokenizer перезатрет
			if (_peekTokens.Count == 0)
				_lookahead = _lookahead.Clone();
			else
				_peekTokens.Last.Value = _peekTokens.Last.Value.Clone();
			var result = _tokenizer.ReadToken();
			_peekTokens.AddLast(result);
			return (result);
		}

		private void MoveForwardLookahead() {
			Contract.Requires(_peekTokens.Count > 0);
			_peekTokens.Clear();
			_lookahead = _tokenizer.ReadToken();
		}

		private Function ParseFunction(bool isDeclaration) {
			Contract.Requires(_currentFunction != null);
			Contract.Ensures(_currentFunction != null);

			var startPosition = _lookahead.StartPosition;

			Match(TokenType.Function);

			string name;
			if (_lookahead.Type == TokenType.Ident) {
				name = _lookahead.Value;
				if (_currentFunction.Outer.NestedFunctions.Contains(name))
					ThrowFunctionAlreadyDeclared(startPosition, name);
				ReadNextToken();
			}
			else {
				name = string.Format(
					"anonymous at {0},{1}",
					startPosition.LineNo.ToString(CultureInfo.InvariantCulture),
					startPosition.ColumnNo.ToString(CultureInfo.InvariantCulture)
				);
			}

			var parameterNames = new VariableCollection();
			Match(TokenType.LParenthesis);
			if (_lookahead.Type != TokenType.RParenthesis) {
				if (_lookahead.Type != TokenType.Ident)
					ThrowUnmatchedToken(TokenType.Ident, _lookahead);
				parameterNames.Add(_lookahead.Value);
				ReadNextToken();
				while (_lookahead.Type == TokenType.Comma) {
					ReadNextToken();
					if (_lookahead.Type != TokenType.Ident)
						ThrowUnmatchedToken(TokenType.Ident, _lookahead);
					var parameterName = _lookahead.Value;
					if (parameterNames.Contains(parameterName))
						ThrowParameterAlreadyDeclared(_lookahead.StartPosition, parameterName);
					parameterNames.Add(parameterName);
					ReadNextToken();
				}
			}
			Match(TokenType.RParenthesis);

			_currentFunction = new FunctionContext(
				_currentFunction, name, parameterNames, new RootStatement(true), isDeclaration
			);
			Match(TokenType.LCurlyBrace);
			ParseStatementList(_currentFunction.RootStatement);
			Match(TokenType.RCurlyBrace);

			var result = _currentFunction.ToFunction();
			_currentFunction = _currentFunction.Outer;
			_currentFunction.NestedFunctions.Add(result);
			return (result);
		}

		public Function ParseGlobal() {
			_currentFunction = new FunctionContext(new RootStatement(false));
			ParseStatementList(_currentFunction.RootStatement);
			Contract.Assert(_lookahead.Type == TokenType.Unknown);
			return (_currentFunction.ToFunction());
		}

		private static IVariableCollection ToVariableCollection(IEnumerable<string> parameterNames) {
			var result = new VariableCollection();
			foreach (var parameterName in parameterNames) {
				if (result.Contains(parameterName))
					ThrowParameterAlreadyDeclared(parameterName);
				result.Add(parameterName);
			}
			return (result);
		}

		public Function ParseFunction(string functionName, IEnumerable<string> parameterNames) {
			Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(functionName), "functionName");
			Contract.Requires<ArgumentNullException>(parameterNames != null, "parameterNames");
			_currentFunction = new FunctionContext(
				functionName, ToVariableCollection(parameterNames), new RootStatement(true)
			);
			ParseStatementList(_currentFunction.RootStatement);
			Contract.Assert(_lookahead.Type == TokenType.Unknown);
			return (_currentFunction.ToFunction());
		}

		#region Errors

		private InvalidTokenException InvalidToken() {
			throw new InvalidTokenException(
				Messages.Error(
					_lookahead.StartPosition.LineNo,
					_lookahead.StartPosition.ColumnNo,
					string.Format(
						"Invalid token \"{0}\".", _lookahead.Type.ToString()
					)
				)
			);
		}

		private static void ThrowUnmatchedToken(TokenType expectedTokenType, Token actualToken) {
			throw new UnmatchedTokenException(
				Messages.Error(
					actualToken.StartPosition.LineNo,
					actualToken.StartPosition.ColumnNo,
					string.Format(
						"Expected \"{0}\" but found \"{1}\".",
						expectedTokenType.ToString(),
						actualToken.Type.ToString()
					)
				)
			);
		}

		private void Match(TokenType expectedTokenType) {
			if (_lookahead.Type != expectedTokenType) {
				ThrowUnmatchedToken(expectedTokenType, _lookahead);
			}
			ReadNextToken();
		}

		private static void ThrowExpectedReference(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo, startPosition.ColumnNo, "Expected reference."
				)
			);
		}

		private static void ThrowExpectedObject(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo, startPosition.ColumnNo, "Expected object."
				)
			);
		}

		private static void ThrowExpectedConstructor(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo, startPosition.ColumnNo, "Expected constructor."
				)
			);
		}

		private static void ThrowExpectedFunction(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo, startPosition.ColumnNo, "Expected function."
				)
			);
		}

		private static void ThrowDuplicatedLabel(TokenPosition position, string label) {
			Contract.Requires(!string.IsNullOrEmpty(label));
			throw new DuplicatedLabelException(
				Messages.Error(
					position.LineNo, position.ColumnNo, string.Format("Duplicated label \"{0}\".", label)
				)
			);
		}

		private static string FormatParameterAlreadyDeclared(string parameterName) {
			return (string.Format("Parameter \"{0}\" was already declared.", parameterName));
		}

		private static void ThrowParameterAlreadyDeclared(string parameterName) {
			Contract.Requires(!string.IsNullOrEmpty(parameterName));
			throw new ParameterAlreadyDeclaredException(
				FormatParameterAlreadyDeclared(parameterName)
			);
		}

		private static void ThrowParameterAlreadyDeclared(TokenPosition position, string parameterName) {
			Contract.Requires(!string.IsNullOrEmpty(parameterName));
			throw new ParameterAlreadyDeclaredException(
				Messages.Error(
					position.LineNo, position.ColumnNo, FormatParameterAlreadyDeclared(parameterName)
				)
			);
		}

		private static void ThrowFunctionAlreadyDeclared(TokenPosition position, string functionName) {
			Contract.Requires(!string.IsNullOrEmpty(functionName));
			throw new FunctionAlreadyDeclaredException(
				Messages.Error(
					position.LineNo, position.ColumnNo, string.Format("Function \"{0}\" was already declared.", functionName)
				)
			);
		}

		private static void ThrowUnreachableLabel(TokenPosition position, string label) {
			Contract.Requires(label != null);
			throw new UnreachableLabelException(
				Messages.Error(
					position.LineNo, position.ColumnNo, string.Format("Can't find target of label \"{0}\".", label)
				)
			);
		}

		private static void ThrowExpectedStatement(TokenPosition position) {
			throw new ExpectedStatementException(
				Messages.Error(
					position.LineNo, position.ColumnNo, "Expected statement."
				)
			);
		}

		private static void ThrowInvalidStatement(TokenPosition position) {
			throw new InvalidStatementException(
				Messages.Error(
					position.LineNo, position.ColumnNo, "Invalid statement."
				)
			);
		}

		private static void ThrowUnexpectedLineTerminator(TokenPosition position) {
			throw new UnexpectedLineTerminatorException(
				Messages.Error(
					position.LineNo, position.ColumnNo, "Unexpected line terminator."
				)
			);
		}

		private static void ThrowExpectedCatchOrFinally(TokenPosition position) {
			throw new ExpectedCatchOrFinallyException(
				Messages.Error(
					position.LineNo, position.ColumnNo, "Expected catch or finally block."
				)
			);
		}

		#endregion
	}
}
