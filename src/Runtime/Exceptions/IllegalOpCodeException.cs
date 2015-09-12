using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Недопустимая текущему состоянию потока исполнения иструкция
	/// </summary>
	[Serializable]
	public class IllegalOpCodeException : InternalErrorException {
		public IllegalOpCodeException() {
		}

		public IllegalOpCodeException(string message)
			: base(message) {
		}

		public IllegalOpCodeException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
