using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Найдены ошибки в синтаксисе
	/// </summary>
	[Serializable]
	public class SyntaxErrorException : RuntimeErrorException {
		public SyntaxErrorException() {
		}

		public SyntaxErrorException(string message)
			: base(message) {
		}

		public SyntaxErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
