using System;

namespace YaJS.Runtime {
	/// <summary>
	/// Базовый класс для всех исключений виртуальной машины
	/// </summary>
	[Serializable]
	public abstract class RuntimeException : Exception {
		public RuntimeException() {
		}

		public RuntimeException(string message)
			: base(message) {
		}

		public RuntimeException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
