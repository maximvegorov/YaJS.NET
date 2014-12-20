using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace YaJS.Compiler.Parser {
	/// <summary>
	/// Входной поток символов
	/// </summary>
	public sealed class CharStream {
		public CharStream(TextReader reader) {
			Contract.Requires(reader != null);
			Reader = reader;
			LineNo = 1;
		}

		/// <summary>
		/// Прочитать очередной символ
		/// </summary>
		public int ReadChar() {
			if (!IsEOF) {
				CurChar = Reader.Read();
				if (CurChar == -1) {
					IsEOF = true;
				}
				else {
					// Преобразовать символы окончания строки в \n
					if (CurChar == '\n' || CurChar == '\r' || CurChar == '\u2028' || CurChar == '\u2029') {
						if (CurChar == '\n') {
							// Преобразовать последовательность \n\r в \n
							if (Reader.Peek() == '\r')
								Reader.Read();
						}
						else
							CurChar = '\n';
						LineNo++;
						ColumnNo = 1;
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
			return (Reader.Peek());
		}

		private TextReader Reader { get; set; }
		public int CurChar { get; private set; }
		public int LineNo { get; private set; }
		public int ColumnNo { get; private set; }
		public bool IsEOF { get; private set; }
	}
}
