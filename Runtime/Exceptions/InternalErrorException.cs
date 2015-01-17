using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Базовый класс для всех внутренних ошибок
	/// </summary>
	[Serializable]
	public abstract class InternalErrorException : RuntimeException {
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
