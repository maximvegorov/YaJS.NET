using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Недопустимое состояние потока выполнения
	/// </summary>
	[Serializable]
	public class IllegalThreadStateException : InternalErrorException {
		public IllegalThreadStateException()
			: base() {
		}

		public IllegalThreadStateException(string message)
			: base(message) {
		}

		public IllegalThreadStateException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
