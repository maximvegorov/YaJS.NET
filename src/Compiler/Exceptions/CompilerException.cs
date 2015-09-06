using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Базовый класс для всех исключений компилятора
	/// </summary>
	[Serializable]
	public abstract class CompilerException : Exception {
		protected CompilerException() {
		}

		protected CompilerException(string message)
			: base(message) {
		}

		protected CompilerException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
