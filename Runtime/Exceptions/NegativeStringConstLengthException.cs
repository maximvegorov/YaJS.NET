using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Обнаружена отрицательная длина строкой константы при чтении byte-кода
	/// </summary>
	[Serializable]
	public sealed class NegativeStringConstLengthException : InternalErrorException {
		public NegativeStringConstLengthException()
			: base() {
		}

		public NegativeStringConstLengthException(string message)
			: base(message) {
		}

		public NegativeStringConstLengthException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
