using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Ожидался оператор
	/// </summary>
	[Serializable]
	public sealed class ExpectedStatementException : ParserException {
		public ExpectedStatementException() {
		}

		public ExpectedStatementException(string message)
			: base(message) {
		}

		public ExpectedStatementException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
