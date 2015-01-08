using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружена недостижимая метка в операторе break/continue
	/// </summary>
	[Serializable]
	public sealed class UnreachableLabelException : ParserException {
		public UnreachableLabelException()
			: base() {
		}

		public UnreachableLabelException(string message)
			: base(message) {
		}

		public UnreachableLabelException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
