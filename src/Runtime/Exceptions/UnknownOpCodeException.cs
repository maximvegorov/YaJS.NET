using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Неизвестный код инструкции
	/// </summary>
	[Serializable]
	public sealed class UnknownOpCodeException : InternalErrorException {
		public UnknownOpCodeException() {
		}

		public UnknownOpCodeException(string message)
			: base(message) {
		}

		public UnknownOpCodeException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
