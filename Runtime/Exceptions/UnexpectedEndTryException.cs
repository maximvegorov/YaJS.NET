using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Обнаружен EndTry, но нет соответствующего ему BeginTry
	/// </summary>
	[Serializable]
	public sealed class UnexpectedEndTryException : InternalErrorException {
		public UnexpectedEndTryException()
			: base() {
		}

		public UnexpectedEndTryException(string message)
			: base(message) {
		}

		public UnexpectedEndTryException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
