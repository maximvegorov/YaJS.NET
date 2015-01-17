using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен недопустимый токен
	/// </summary>
	[Serializable]
	public sealed class InvalidTokenException : ParserException {
		public InvalidTokenException() {
		}

		public InvalidTokenException(string message)
			: base(message) {
		}

		public InvalidTokenException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
