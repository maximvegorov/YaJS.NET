using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace YaJS.Compiler {
	/// <summary>
	/// Входной поток символов
	/// </summary>
	public sealed class CharStream {
		private readonly TextReader _reader;

		public CharStream(TextReader reader) {
			Contract.Requires<ArgumentNullException>(reader != null, "reader");
			_reader = reader;
			LineNo = 1;
			// ВАЖНО!!! Всегда надо иметь один прочитанный вперед символ
			ReadChar();
		}

		/// <summary>
		/// Прочитать очередной символ
		/// </summary>
		public void ReadChar() {
			if (!IsEOF) {
				CurChar = _reader.Read();
#if DEBUG
				Offset++;
#endif
				switch (CurChar) {
					case -1:
						IsEOF = true;
						break;
					case '\n':
					case '\r':
					case '\u2028':
					case '\u2029':
						if (CurChar == '\n') {
							// Преобразовать последовательность \n\r в \n
							if (_reader.Peek() == '\r')
								_reader.Read();
						}
						else
							CurChar = '\n';
						LineNo++;
						ColumnNo = 1;
						break;
					default:
						ColumnNo++;
						break;
				}
			}
		}

		/// <summary>
		/// Подсмотреть следующий символ
		/// </summary>
		public int PeekChar() {
			return (_reader.Peek());
		}

		public int CurChar { get; private set; }
#if DEBUG
		public int Offset { get; private set; }
#endif
		public int LineNo { get; private set; }
		public int ColumnNo { get; private set; }
		public bool IsEOF { get; private set; }
	}
}
