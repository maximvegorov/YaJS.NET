using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружена дублирующаяся метка
	/// </summary>
	[Serializable]
	public sealed class DuplicatedLabelException : ParserException {
		public DuplicatedLabelException()
			: base() {
		}

		public DuplicatedLabelException(string message)
			: base(message) {
		}

		public DuplicatedLabelException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
