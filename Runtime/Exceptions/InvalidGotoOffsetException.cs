using System;

namespace YaJS.Runtime.Exceptions {
	[Serializable]
	public sealed class InvalidGotoOffsetException : InternalErrorException {
		public InvalidGotoOffsetException()
			: base() {
		}

		public InvalidGotoOffsetException(string message)
			: base(message) {
		}

		public InvalidGotoOffsetException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
