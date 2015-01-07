using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Функция с таким именем уже была объявлена
	/// </summary>
	[Serializable]
	public sealed class FunctionAlreadyDeclaredException : ParserException {
		public FunctionAlreadyDeclaredException()
			: base() {
		}

		public FunctionAlreadyDeclaredException(string message)
			: base(message) {
		}

		public FunctionAlreadyDeclaredException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
