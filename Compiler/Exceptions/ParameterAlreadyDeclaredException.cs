using System;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Параметр с таким именем уже был объявлен
	/// </summary>
	[Serializable]
	public sealed class ParameterAlreadyDeclaredException : ParserException {
		public ParameterAlreadyDeclaredException() {
		}

		public ParameterAlreadyDeclaredException(string message)
			: base(message) {
		}

		public ParameterAlreadyDeclaredException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
