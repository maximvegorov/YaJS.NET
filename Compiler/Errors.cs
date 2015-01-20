using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using YaJS.Compiler.Exceptions;

namespace YaJS.Compiler {
	internal static class Errors {
		private static string FormatError(int lineNo, string message) {
			Contract.Requires(lineNo > 0);
			Contract.Requires(!string.IsNullOrEmpty(message));
			return (string.Format(
				"[Error] - {0} at {1}",
				message,
				lineNo.ToString(CultureInfo.InvariantCulture)
				));
		}

		private static string FormatError(int lineNo, int columnNo, string message) {
			Contract.Requires(lineNo > 0);
			Contract.Requires(columnNo > 0);
			Contract.Requires(!string.IsNullOrEmpty(message));
			return (string.Format(
				"[Error] - {0} at {1},{2}",
				message,
				lineNo.ToString(CultureInfo.InvariantCulture),
				columnNo.ToString(CultureInfo.InvariantCulture)
				));
		}

		internal static string FormatError(TokenPosition position, string message) {
			return (FormatError(position.LineNo, position.ColumnNo, message));
		}

		internal static void ThrowInternalError(
			[CallerFilePath] string filePath = null,
			[CallerLineNumber] int lineNo = 0,
			[CallerMemberName] string memberName = null
			) {
			throw new InternalErrorException(
				string.Format("{0} at line {1} ({2})", filePath, lineNo.ToString(CultureInfo.InvariantCulture), memberName)
				);
		}

		internal static void ThrowUnexpectedChar(int lineNo, int columnNo, int c) {
			throw new UnexpectedCharException(
				FormatError(
					lineNo,
					columnNo,
					string.Format("Unexpected char \"{0}\".", (char)c)
					)
				);
		}

		internal static void ThrowUnexpectedEndOfFile(int lineNo, int columnNo) {
			throw new UnexpectedEndOfFileException(
				FormatError(
					lineNo,
					columnNo,
					"Unexpected end of file."
					)
				);
		}

		internal static InvalidTokenException InvalidToken(Token token) {
			throw new InvalidTokenException(
				FormatError(
					token.StartPosition.LineNo,
					token.StartPosition.ColumnNo,
					string.Format(
						"Invalid token \"{0}\".",
						token.Type
						)
					)
				);
		}

		internal static void ThrowUnmatchedToken(TokenType expectedTokenType, Token actualToken) {
			throw new UnmatchedTokenException(
				FormatError(
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

		internal static void ThrowExpectedReference(TokenPosition position) {
			throw new ExpectedConstructorException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Expected reference."
					)
				);
		}

		internal static void ThrowExpectedObject(TokenPosition position) {
			throw new ExpectedConstructorException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Expected object."
					)
				);
		}

		internal static void ThrowExpectedConstructor(TokenPosition position) {
			throw new ExpectedConstructorException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Expected constructor."
					)
				);
		}

		internal static void ThrowExpectedFunction(TokenPosition position) {
			throw new ExpectedConstructorException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Expected function."
					)
				);
		}

		internal static void ThrowDuplicatedLabel(TokenPosition position, string label) {
			Contract.Requires(!string.IsNullOrEmpty(label));
			throw new DuplicatedLabelException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					string.Format("Duplicated label \"{0}\".", label)
					)
				);
		}

		private static string FormatParameterAlreadyDeclared(string parameterName) {
			return (string.Format("Parameter \"{0}\" was already declared.", parameterName));
		}

		internal static void ThrowParameterAlreadyDeclared(string parameterName) {
			Contract.Requires(!string.IsNullOrEmpty(parameterName));
			throw new ParameterAlreadyDeclaredException(
				FormatParameterAlreadyDeclared(parameterName)
				);
		}

		internal static void ThrowParameterAlreadyDeclared(TokenPosition position, string parameterName) {
			Contract.Requires(!string.IsNullOrEmpty(parameterName));
			throw new ParameterAlreadyDeclaredException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					FormatParameterAlreadyDeclared(parameterName)
					)
				);
		}

		internal static void ThrowFunctionAlreadyDeclared(TokenPosition position, string functionName) {
			Contract.Requires(!string.IsNullOrEmpty(functionName));
			throw new FunctionAlreadyDeclaredException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					string.Format("Function \"{0}\" was already declared.", functionName)
					)
				);
		}

		internal static void ThrowUnreachableLabel(int lineNo, string label) {
			Contract.Requires(label != null);
			throw new UnreachableLabelException(
				FormatError(
					lineNo,
					string.Format("Can't find target of label \"{0}\".", label)
					)
				);
		}

		internal static void ThrowExpectedStatement(TokenPosition position) {
			throw new ExpectedStatementException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Expected statement."
					)
				);
		}

		internal static void ThrowInvalidStatement(TokenPosition position) {
			throw new InvalidStatementException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Invalid statement."
					)
				);
		}

		internal static void ThrowUnexpectedLineTerminator(TokenPosition position) {
			throw new UnexpectedLineTerminatorException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Unexpected line terminator."
					)
				);
		}

		internal static void ThrowExpectedCatchOrFinally(TokenPosition position) {
			throw new ExpectedCatchOrFinallyException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Expected catch or finally block."
					)
				);
		}

		internal static void ThrowUnsupportedCaseClauseExpression(TokenPosition position) {
			throw new UnsupportedCaseClauseExpressionException(
				FormatError(
					position.LineNo,
					position.ColumnNo,
					"Unsupported case clause expression."
					)
				);
		}

		internal static void ThrowExpectedCaseClause(TokenPosition position) {
			throw new ExpectedCaseClauseException(
				FormatError(
					position.LineNo,
					"Expected case clause."
					)
				);
		}
	}
}
