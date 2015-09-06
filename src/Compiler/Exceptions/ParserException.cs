using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Базовый класс для всех исключений парсера
	/// </summary>
	[Serializable]
	public abstract class ParserException : CompilerException {
		protected ParserException() {
		}

		protected ParserException(string message)
			: base(message) {
		}

		protected ParserException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
