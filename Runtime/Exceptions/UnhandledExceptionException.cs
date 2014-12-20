using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Обнаружено необработанное исключение в JS-коде
	/// </summary>
	[Serializable]
	public sealed class UnhandledExceptionException : InternalErrorException {
		public UnhandledExceptionException()
			: base() {
		}

		public UnhandledExceptionException(string message)
			: base(message) {
		}

		public UnhandledExceptionException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
