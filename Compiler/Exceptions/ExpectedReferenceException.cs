using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Ожидалась ссылка
	/// </summary>
	[Serializable]
	public sealed class ExpectedReferenceException : ParserException {
		public ExpectedReferenceException() {
		}

		public ExpectedReferenceException(string message)
			: base(message) {
		}

		public ExpectedReferenceException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
