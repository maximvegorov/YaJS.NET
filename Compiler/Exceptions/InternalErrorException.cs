using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Внутренняя ошибка компилятора
	/// </summary>
	[Serializable]
	public sealed class InternalErrorException : ParserException {
		public InternalErrorException() {
		}

		public InternalErrorException(string message)
			: base(message) {
		}

		public InternalErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
