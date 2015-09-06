using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Базовый класс для всех исключений сканера
	/// </summary>
	[Serializable]
	public abstract class TokenizerException : CompilerException {
		protected TokenizerException() {
		}

		protected TokenizerException(string message)
			: base(message) {
		}

		protected TokenizerException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
