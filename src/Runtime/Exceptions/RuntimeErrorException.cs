using System;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// Базовый класс всех восстанавливаемых исключений. Исключения данного типа могут быть использованы
	/// в реализациях JSNativeFunction и в случае возникновения будут преобразованы в соотвествующий
	/// класс потомок JSError
	/// </summary>
	[Serializable]
	public abstract class RuntimeErrorException : RuntimeException {
	    protected RuntimeErrorException() {
		}

	    protected RuntimeErrorException(string message)
			: base(message) {
		}

	    protected RuntimeErrorException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
