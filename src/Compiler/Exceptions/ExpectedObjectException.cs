using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Ожидался объект
	/// </summary>
	[Serializable]
	public sealed class ExpectedObjectException : ParserException {
		public ExpectedObjectException() {
		}

		public ExpectedObjectException(string message)
			: base(message) {
		}

		public ExpectedObjectException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
