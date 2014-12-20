using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Недопустимое состояние потока выполнения
	/// </summary>
	[Serializable]
	public class InvalidThreadStateException : InternalErrorException {
		public InvalidThreadStateException()
			: base() {
		}

		public InvalidThreadStateException(string message)
			: base(message) {
		}

		public InvalidThreadStateException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
