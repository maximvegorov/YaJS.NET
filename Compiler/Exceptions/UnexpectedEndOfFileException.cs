using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен преждевременный конец файла
	/// </summary>
	[Serializable]
	public sealed class UnexpectedEndOfFileException : ParserException {
		public UnexpectedEndOfFileException()
			: base() {
		}

		public UnexpectedEndOfFileException(string message)
			: base(message) {
		}

		public UnexpectedEndOfFileException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
