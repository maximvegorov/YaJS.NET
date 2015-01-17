using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Ожидалась функция
	/// </summary>
	[Serializable]
	public sealed class ExpectedFunctionException : ParserException {
		public ExpectedFunctionException() {
		}

		public ExpectedFunctionException(string message)
			: base(message) {
		}

		public ExpectedFunctionException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
