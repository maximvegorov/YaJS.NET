using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Данное выражение не поддерживается в Selector-а для CaseClause
	/// </summary>
	[Serializable]
	public sealed class UnsupportedCaseClauseExpressionException : ParserException {
		public UnsupportedCaseClauseExpressionException()
			: base() {
		}

		public UnsupportedCaseClauseExpressionException(string message)
			: base(message) {
		}

		public UnsupportedCaseClauseExpressionException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
