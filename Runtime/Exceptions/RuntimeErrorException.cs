using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Базовый класс всех восстанавливаемых исключений. Исключения данного типа могут быть использованы
	/// в реализациях JSNativeFunction и в случае возникновения будут преобразованы в JSRuntimeError
	/// </summary>
	[Serializable]
	public abstract class RuntimeErrorException : RuntimeException {
		public RuntimeErrorException() {
		}

		public RuntimeErrorException(string message)
			: base(message) {
		}

		public RuntimeErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
