using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;
using YaJS.Compiler.Exceptions;

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

		private Function ParseFunction(bool isDeclaration) {
			Contract.Requires(_currentFunction != null);
			Contract.Ensures(_currentFunction != null);

			var startPosition = Lookahead.StartPosition;

			Match(TokenType.Function);

			string name = null;
			if (Lookahead.Type == TokenType.Ident) {
				name = Lookahead.Value;
				if (_currentFunction.Outer.NestedFunctions.Contains(name))
					ThrowFunctionAlreadyDeclared(startPosition, name);
				ReadNextToken();
			}

			var parameterNames = new VariableCollection();
			Match(TokenType.LParenthesis);
			if (Lookahead.Type != TokenType.RParenthesis) {
				if (Lookahead.Type != TokenType.Ident)
					ThrowUnmatchedToken(TokenType.Ident, Lookahead);
				parameterNames.Add(Lookahead.Value);
				ReadNextToken();
				while (Lookahead.Type == TokenType.Comma) {
					ReadNextToken();
					if (Lookahead.Type != TokenType.Ident)
						ThrowUnmatchedToken(TokenType.Ident, Lookahead);
					var parameterName = Lookahead.Value;
					if (parameterNames.Contains(parameterName))
						ThrowParameterAlreadyDeclared(Lookahead.StartPosition, parameterName);
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
				new FunctionBody(),
				isDeclaration
				);
			Match(TokenType.LCurlyBrace);
			ParseFunctionBody(_currentFunction.FunctionBody);
			Match(TokenType.RCurlyBrace);

			var result = _currentFunction.ToFunction();
			_currentFunction = _currentFunction.Outer;
			_currentFunction.NestedFunctions.Add(result);
			return (result);
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
			_currentFunction = new FunctionBuilder(
				null,
				functionName,
				1,
				ToVariableCollection(parameterNames),
				new FunctionBody(),
				false
				);
			ParseFunctionBody(_currentFunction.FunctionBody);
			Contract.Assert(Lookahead.Type == TokenType.Unknown);
			return (_currentFunction.ToFunction());
		}

		public Token Lookahead { get; private set; }

		#region Errors

		private InvalidTokenException InvalidToken() {
			throw new InvalidTokenException(
				Messages.Error(
					Lookahead.StartPosition.LineNo,
					Lookahead.StartPosition.ColumnNo,
					string.Format(
						"Invalid token \"{0}\".",
						Lookahead.Type
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
						expectedTokenType,
						actualToken.Type
						)
					)
				);
		}

		private void Match(TokenType expectedTokenType) {
			if (Lookahead.Type != expectedTokenType)
				ThrowUnmatchedToken(expectedTokenType, Lookahead);
			ReadNextToken();
		}

		private static void ThrowExpectedReference(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo,
					startPosition.ColumnNo,
					"Expected reference."
					)
				);
		}

		private static void ThrowExpectedObject(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo,
					startPosition.ColumnNo,
					"Expected object."
					)
				);
		}

		private static void ThrowExpectedConstructor(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo,
					startPosition.ColumnNo,
					"Expected constructor."
					)
				);
		}

		private static void ThrowExpectedFunction(TokenPosition startPosition) {
			throw new ExpectedConstructorException(
				Messages.Error(
					startPosition.LineNo,
					startPosition.ColumnNo,
					"Expected function."
					)
				);
		}

		private static void ThrowDuplicatedLabel(TokenPosition position, string label) {
			Contract.Requires(!string.IsNullOrEmpty(label));
			throw new DuplicatedLabelException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					string.Format("Duplicated label \"{0}\".", label)
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
					position.LineNo,
					position.ColumnNo,
					FormatParameterAlreadyDeclared(parameterName)
					)
				);
		}

		private static void ThrowFunctionAlreadyDeclared(TokenPosition position, string functionName) {
			Contract.Requires(!string.IsNullOrEmpty(functionName));
			throw new FunctionAlreadyDeclaredException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					string.Format("Function \"{0}\" was already declared.", functionName)
					)
				);
		}

		private static void ThrowUnreachableLabel(TokenPosition position, string label) {
			Contract.Requires(label != null);
			throw new UnreachableLabelException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					string.Format("Can't find target of label \"{0}\".", label)
					)
				);
		}

		private static void ThrowExpectedStatement(TokenPosition position) {
			throw new ExpectedStatementException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					"Expected statement."
					)
				);
		}

		private static void ThrowInvalidStatement(TokenPosition position) {
			throw new InvalidStatementException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					"Invalid statement."
					)
				);
		}

		private static void ThrowUnexpectedLineTerminator(TokenPosition position) {
			throw new UnexpectedLineTerminatorException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					"Unexpected line terminator."
					)
				);
		}

		private static void ThrowExpectedCatchOrFinally(TokenPosition position) {
			throw new ExpectedCatchOrFinallyException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					"Expected catch or finally block."
					)
				);
		}

		private static void ThrowUnsupportedCaseClauseExpression(TokenPosition position) {
			throw new UnsupportedCaseClauseExpressionException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					"Unsupported case clause expression."
					)
				);
		}

		private static void ThrowExpectedCaseClause(TokenPosition position) {
			throw new ExpectedCaseClauseException(
				Messages.Error(
					position.LineNo,
					position.ColumnNo,
					"Expected case clause."
					)
				);
		}

		#endregion
	}
}
