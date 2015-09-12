using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Ожидался хотя бы один case в операторе switch
	/// </summary>
	[Serializable]
	public sealed class ExpectedCaseClauseException : ParserException {
		public ExpectedCaseClauseException() {
		}

		public ExpectedCaseClauseException(string message)
			: base(message) {
		}

		public ExpectedCaseClauseException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
