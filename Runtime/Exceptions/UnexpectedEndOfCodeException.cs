using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Код функции не валиден. Ожидалось продолжение кода
	/// </summary>
	[Serializable]
	public sealed class UnexpectedEndOfCodeException : InternalErrorException {
		public UnexpectedEndOfCodeException() {
		}

		public UnexpectedEndOfCodeException(string message)
			: base(message) {
		}

		public UnexpectedEndOfCodeException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
