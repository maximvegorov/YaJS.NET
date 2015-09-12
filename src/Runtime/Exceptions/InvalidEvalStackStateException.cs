using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Недопустимое состояние стека вычислений
	/// </summary>
	[Serializable]
	public sealed class InvalidEvalStackStateException : InternalErrorException {
		public InvalidEvalStackStateException() {
		}

		public InvalidEvalStackStateException(string message)
			: base(message) {
		}

		public InvalidEvalStackStateException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
