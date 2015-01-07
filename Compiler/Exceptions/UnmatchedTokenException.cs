using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaJS.Compiler.Exceptions {
	/// <summary>
	/// Обнаружен несоотвествующий ожидаемому токен
	/// </summary>
	[Serializable]
	public sealed class UnmatchedTokenException : ParserException {
		public UnmatchedTokenException()
			: base() {
		}

		public UnmatchedTokenException(string message)
			: base(message) {
		}

		public UnmatchedTokenException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
