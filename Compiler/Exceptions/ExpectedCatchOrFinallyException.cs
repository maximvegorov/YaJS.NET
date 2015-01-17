using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// У оператора try отсутствуют блоки catch и finally
	/// </summary>
	[Serializable]
	public sealed class ExpectedCatchOrFinallyException : ParserException {
		public ExpectedCatchOrFinallyException() {
		}

		public ExpectedCatchOrFinallyException(string message)
			: base(message) {
		}

		public ExpectedCatchOrFinallyException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
