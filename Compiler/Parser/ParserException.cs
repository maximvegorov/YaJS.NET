using System;

namespace LementPro.Core.Expressions {
	[Serializable]
	public sealed class ParserException : Exception {
		public ParserException()
			: base() {
		}

		public ParserException(string message)
			: base(message) {
		}

		public ParserException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
