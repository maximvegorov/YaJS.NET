using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Ожидался конструктор
	/// </summary>
	[Serializable]
	public sealed class ExpectedConstructorException : ParserException {
		public ExpectedConstructorException() {
		}

		public ExpectedConstructorException(string message)
			: base(message) {
		}

		public ExpectedConstructorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
