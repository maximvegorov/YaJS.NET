using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Недопустимый операнд для операции
	/// </summary>
	[Serializable]
	public sealed class InvalidOperandTypeException : ParserException {
		public InvalidOperandTypeException() {
		}

		public InvalidOperandTypeException(string message)
			: base(message) {
		}

		public InvalidOperandTypeException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
