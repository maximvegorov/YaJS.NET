using System.Diagnostics.Contracts;
using System.IO;

namespace YaJS.Compiler {
	/// <summary>
	/// Входной поток символов
	/// </summary>
	public sealed class CharStream {
		private TextReader _reader;

		public CharStream(TextReader reader) {
			Contract.Requires(reader != null);
			_reader = reader;
			LineNo = 1;
			// ВАЖНО!!! Всегда надо иметь один прочитанный вперед символ
			ReadChar();
		}

		/// <summary>
		/// Прочитать очередной символ
		/// </summary>
		public int ReadChar() {
			if (!IsEOF) {
				CurChar = _reader.Read();
#if DEBUG
				Offset++;
#endif
				if (CurChar == -1) {
					IsEOF = true;
				}
				else {
					// Преобразовать символы окончания строки в \n
					if (CurChar == '\n' || CurChar == '\r' || CurChar == '\u2028' || CurChar == '\u2029') {
						if (CurChar == '\n') {
							// Преобразовать последовательность \n\r в \n
							if (_reader.Peek() == '\r')
								_reader.Read();
						}
						else
							CurChar = '\n';
						LineNo++;
						ColumnNo = 0;
					}
					else
						ColumnNo++;
				}
			}
			return (CurChar);
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
