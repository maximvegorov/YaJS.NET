using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Несоответствующее типу использование JSValue
	/// </summary>
	[Serializable]
	public sealed class TypeErrorException : RuntimeErrorException {
		public TypeErrorException()
			: base() {
		}

		public TypeErrorException(string message)
			: base(message) {
		}

		public TypeErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
