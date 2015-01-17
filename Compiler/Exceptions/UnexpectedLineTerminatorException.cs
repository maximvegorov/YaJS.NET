using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен запрещенный перенос строки
	/// </summary>
	[Serializable]
	public sealed class UnexpectedLineTerminatorException : ParserException {
		public UnexpectedLineTerminatorException() {
		}

		public UnexpectedLineTerminatorException(string message)
			: base(message) {
		}

		public UnexpectedLineTerminatorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
