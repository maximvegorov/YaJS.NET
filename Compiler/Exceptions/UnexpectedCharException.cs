using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен недопустимый символ
	/// </summary>
	[Serializable]
	public sealed class UnexpectedCharException : ParserException {
		public UnexpectedCharException() {
		}

		public UnexpectedCharException(string message)
			: base(message) {
		}

		public UnexpectedCharException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
