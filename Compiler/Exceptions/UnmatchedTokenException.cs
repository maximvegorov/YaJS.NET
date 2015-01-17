using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен несоотвествующий ожидаемому токен
	/// </summary>
	[Serializable]
	public sealed class UnmatchedTokenException : ParserException {
		public UnmatchedTokenException() {
		}

		public UnmatchedTokenException(string message)
			: base(message) {
		}

		public UnmatchedTokenException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
