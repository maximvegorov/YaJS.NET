using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Попытка доступа к несуществующей переменной
	/// </summary>
	[Serializable]
	public class ReferenceErrorException : RuntimeErrorException {
		public ReferenceErrorException() {
		}

		public ReferenceErrorException(string message)
			: base(message) {
		}

		public ReferenceErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
