using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Недопустимое имя переменной
	/// </summary>
	[Serializable]
	public sealed class InvalidVariableNameException : InternalErrorException {
		public InvalidVariableNameException() {
		}

		public InvalidVariableNameException(string message)
			: base(message) {
		}

		public InvalidVariableNameException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
