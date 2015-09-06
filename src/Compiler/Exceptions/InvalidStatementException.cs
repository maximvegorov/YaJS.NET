using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен недопустимый оператор
	/// </summary>
	[Serializable]
	public sealed class InvalidStatementException : ParserException {
		public InvalidStatementException() {
		}

		public InvalidStatementException(string message)
			: base(message) {
		}

		public InvalidStatementException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
