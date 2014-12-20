using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Базовый класс для всех исключений парсера
	/// </summary>
	[Serializable]
	public class ParserException : Exception {
		public ParserException()
			: base() {
		}

		public ParserException(string message)
			: base(message) {
		}

		public ParserException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
