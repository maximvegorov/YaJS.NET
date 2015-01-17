using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Значение за пределами допустимого диапазона
	/// </summary>
	[Serializable]
	public sealed class RangeErrorException : RuntimeErrorException {
		public RangeErrorException() {
		}

		public RangeErrorException(string message)
			: base(message) {
		}

		public RangeErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
