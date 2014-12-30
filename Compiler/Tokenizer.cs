using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace YaJS.Compiler {
	using YaJS.Compiler.Exceptions;
	using YaJS.Compiler.Helpers;

	/// <summary>
	/// Сканер
	/// </summary>
	public sealed class Tokenizer {
		private static readonly Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>();

		static Tokenizer() {
			Keywords.Add("break", TokenType.Break);
			Keywords.Add("case", TokenType.Case);
			Keywords.Add("catch", TokenType.Catch);
			Keywords.Add("continue", TokenType.Continue);
			Keywords.Add("debugger", TokenType.Debugger);
			Keywords.Add("default", TokenType.Default);
			Keywords.Add("delete", TokenType.Delete);
			Keywords.Add("do", TokenType.Do);
			Keywords.Add("else", TokenType.Else);
			Keywords.Add("false", TokenType.False);
			Keywords.Add("finally", TokenType.Finally);
			Keywords.Add("for", TokenType.For);
			Keywords.Add("function", TokenType.Function);
			Keywords.Add("if", TokenType.If);
			Keywords.Add("in", TokenType.In);
			Keywords.Add("instanceof", TokenType.InstanceOf);
			Keywords.Add("null", TokenType.Null);
			Keywords.Add("new", TokenType.New);
			Keywords.Add("return", TokenType.Return);
			Keywords.Add("switch", TokenType.Switch);
			Keywords.Add("this", TokenType.This);
			Keywords.Add("throw", TokenType.Throw);
			Keywords.Add("true", TokenType.True);
			Keywords.Add("try", TokenType.Try);
			Keywords.Add("typeof", TokenType.Typeof);
			Keywords.Add("var", TokenType.Var);
			Keywords.Add("void", TokenType.Void);
			Keywords.Add("while", TokenType.While);
			Keywords.Add("with", TokenType.With);

			Keywords.Add("class", TokenType.Class);
			Keywords.Add("enum", TokenType.Enum);
			Keywords.Add("extends", TokenType.Extends);
			Keywords.Add("super", TokenType.Super);
			Keywords.Add("const", TokenType.Const);
			Keywords.Add("export", TokenType.Export);
			Keywords.Add("import", TokenType.Import);
			Keywords.Add("implements", TokenType.Implements);
			Keywords.Add("let", TokenType.Let);
			Keywords.Add("private", TokenType.Private);
			Keywords.Add("public", TokenType.Public);
			Keywords.Add("yield", TokenType.Yield);
			Keywords.Add("interface", TokenType.Interface);
			Keywords.Add("package", TokenType.Package);
			Keywords.Add("protected", TokenType.Protected);
			Keywords.Add("static", TokenType.Static);
		}

		public Tokenizer(TextReader source) {
			Contract.Requires(source != null);
			Source = new CharStream(source);
			CurToken = new Token();
		}

		private static bool IsIdentStartChar(char c) {
			return (char.IsLetter(c) || c == '$' || c == '_');
		}

		private void ReadIdentOrKeyword() {
			StringBuilder builder = new StringBuilder();
			builder.Append((char)Source.CurChar);
			Source.ReadChar();
			while (char.IsLetterOrDigit((char)Source.CurChar) || Source.CurChar == '$' || Source.CurChar == '_' || Source.CurChar == '\u200C' || Source.CurChar == '\u200D') {
				builder.Append((char)Source.CurChar);
				Source.ReadChar();
			}
			var value = builder.ToString();
			TokenType keyword;
			if (Keywords.TryGetValue(value, out keyword))
				CurToken.Type = keyword;
			else {
				CurToken.Type = TokenType.Ident;
				CurToken.Value = value;
			}
		}

		private void Read_Lt_or_Lte_or_Shl_or_ShlAssign() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.Lte;
				Source.ReadChar();
			}
			else if (Source.CurChar == '<') {
				Source.ReadChar();
				if (Source.CurChar == '=') {
					CurToken.Type = TokenType.ShlAssign;
					Source.ReadChar();
				}
				else
					CurToken.Type = TokenType.Shl;
			}
			else
				CurToken.Type = TokenType.Lt;
		}

		private void Read_Gt_or_Gte_or_ShrS_or_ShrSAssign_or_ShrU_or_ShrUAssign() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.Gte;
				Source.ReadChar();
			}
			else if (Source.CurChar == '>') {
				Source.ReadChar();
				if (Source.CurChar == '=') {
					CurToken.Type = TokenType.ShrSAssign;
					Source.ReadChar();
				}
				else if (Source.CurChar == '>') {
					Source.ReadChar();
					if (Source.CurChar == '=') {
						CurToken.Type = TokenType.ShrUAssign;
						Source.ReadChar();
					}
					else
						CurToken.Type = TokenType.ShrU;
				}
				else
					CurToken.Type = TokenType.ShrS;
			}
			else
				CurToken.Type = TokenType.Gt;
		}

		private void Read_Eq_or_Assign_or_StrictEq() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				Source.ReadChar();
				if (Source.CurChar == '=') {
					CurToken.Type = TokenType.StrictEq;
					Source.ReadChar();
				}
				else
					CurToken.Type = TokenType.Eq;
			}
			else
				CurToken.Type = TokenType.Assign;
		}

		private void Read_Plus_or_PlusAssign_or_Inc() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.PlusAssign;
				Source.ReadChar();
			}
			else if (Source.CurChar == '+') {
				CurToken.Type = TokenType.Inc;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.Plus;
		}

		private void Read_Minus_or_MinusAssign_or_Dec() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.MinusAssign;
				Source.ReadChar();
			}
			else if (Source.CurChar == '-') {
				CurToken.Type = TokenType.Dec;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.Minus;
		}

		private void Read_Star_or_StarAssign() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.StarAssign;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.Star;
		}

		private void SkipSingleLineComment() {
			do {
				Source.ReadChar();
			}
			while (Source.CurChar != '\n' && Source.CurChar != -1);
		}

		private void ThrowUnexpectedEndOfFile() {
			throw new UnexpectedEndOfFileException(
				LogHelper.Error(
					Source.LineNo,
					Source.ColumnNo,
					"Unexpected end of file"
				)
			);
		}

		private void SkipMultiLineComment() {
			for(;;) {
				Source.ReadChar();
				switch (Source.CurChar) {
					case -1:
						ThrowUnexpectedEndOfFile();
						break;
					case '\n':
						// Если многострочный комментарий содержит символ окончания строки,
						// то считаем что следующей лексеме предшествует символ окончания строки
						// см. http://ecma-international.org/ecma-262/5.1/#sec-7.4
						CurToken.IsAfterLineTerminator = true;
						break;
					case '*':
						Source.ReadChar();
						if (Source.CurChar == '/') {
							Source.ReadChar();
							return;
						}
						break;
				}
			}
		}

		private void TryRead_Slash_or_SlashAssign(bool isRegexAllowed) {
			Source.ReadChar();
			if (!isRegexAllowed && Source.CurChar == '=') {
				CurToken.Type = TokenType.SlashAssign;
				Source.ReadChar();
			}
			else if (Source.CurChar == '/')
				SkipSingleLineComment();
			else if (Source.CurChar == '*')
				SkipMultiLineComment();
			else
				CurToken.Type = TokenType.Slash;
		}

		private void Read_Mod_or_ModAssign() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.ModAssign;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.Mod;
		}

		private void Read_BitAnd_or_BitAndAssign_or_And() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.BitAndAssign;
				Source.ReadChar();
			}
			else if (Source.CurChar == '&') {
				CurToken.Type = TokenType.And;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.BitAnd;
		}

		private void Read_BitOr_or_BitOrAssign_or_Or() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.BitOrAssign;
				Source.ReadChar();
			}
			else if (Source.CurChar == '|') {
				CurToken.Type = TokenType.Or;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.BitOr;
		}

		private void Read_BitXor_or_BitXorAssign() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				CurToken.Type = TokenType.BitXorAssign;
				Source.ReadChar();
			}
			else
				CurToken.Type = TokenType.BitXor;
		}

		private void Read_Not_or_Neq_or_StrictNeq() {
			Source.ReadChar();
			if (Source.CurChar == '=') {
				Source.ReadChar();
				if (Source.CurChar == '=') {
					CurToken.Type = TokenType.StrictNeq;
					Source.ReadChar();
				}
				else
					CurToken.Type = TokenType.Neq;
			}
			else
				CurToken.Type = TokenType.Not;
		}

		private void ThrowUnexpectedChar() {
			throw new UnexpectedCharException(
				LogHelper.Error(
					Source.LineNo,
					Source.ColumnNo,
					string.Format("Unexpected char \"{0}\"", Source.CurChar.ToString())
				)
			);
		}

		private void ReadIntegerOrFloatNumber() {
			CurToken.Type = TokenType.Integer;
			StringBuilder builder = new StringBuilder();
			while (char.IsDigit((char)Source.CurChar)) {
				builder.Append((char)Source.CurChar);
				Source.ReadChar();
			}
			// Ноль может быть удален при попытке распознать HexInteger
			if (builder.Length == 0)
				builder.Append('0');
			if (Source.CurChar == '.') {
				CurToken.Type = TokenType.Float;
				builder.Append((char)Source.CurChar);
				Source.ReadChar();
				if (!char.IsDigit((char)Source.CurChar))
					ThrowUnexpectedChar();
				do {
					builder.Append((char)Source.CurChar);
					Source.ReadChar();
				} while (char.IsDigit((char)Source.CurChar));
			}
			if (Source.CurChar == 'e' || Source.CurChar == 'E') {
				CurToken.Type = TokenType.Float;
				builder.Append((char)Source.CurChar);
				Source.ReadChar();
				if (Source.CurChar == '-' || Source.CurChar == '+') {
					builder.Append((char)Source.CurChar);
					Source.ReadChar();
				}
				if (!char.IsDigit((char)Source.CurChar))
					ThrowUnexpectedChar();
				do {
					builder.Append((char)Source.CurChar);
					Source.ReadChar();
				} while (char.IsDigit((char)Source.CurChar));
			}
			// Число и идентификатор должны быть разделены хотя бы одним символом
			// см. http://ecma-international.org/ecma-262/5.1/#sec-7.8.3
			if (IsIdentStartChar((char)Source.CurChar))
				ThrowUnexpectedChar();
			CurToken.Value = builder.ToString();
		}

		private static bool IsHexDigit(char c) {
			return (char.IsDigit(c) || ('a' <= c && c <= 'f') || ('A' <= c && c <= 'F'));
		}

		private void ReadHexIntegerNumber() {
			// Пропустить x или X
			Source.ReadChar();
			if (!IsHexDigit((char)Source.CurChar))
				ThrowUnexpectedChar();
			StringBuilder builder = new StringBuilder();
			do {
				builder.Append((char)Source.CurChar);
				Source.ReadChar();
			}
			while (IsHexDigit((char)Source.CurChar));
			// Число и идентификатор должны быть разделены хотя бы одним символом
			// см. http://ecma-international.org/ecma-262/5.1/#sec-7.8.3
			if (IsIdentStartChar((char)Source.CurChar))
				ThrowUnexpectedChar();
			CurToken.Type = TokenType.HexInteger;
			CurToken.Value = builder.ToString();
		}

		private char ReadEscapeSequence(int length) {
			Source.ReadChar();
			int result = 0;
			for (int i = 0; i < length; i++) {
				int hexDigit = 0;
				if ('0' <= Source.CurChar && Source.CurChar <= '9')
					hexDigit = Source.CurChar - '0';
				else if ('a' <= Source.CurChar && Source.CurChar <= 'f')
					hexDigit = Source.CurChar - 'a' + 10;
				else if ('A' <= Source.CurChar && Source.CurChar <= 'F')
					hexDigit = Source.CurChar - 'A' + 10;
				else
					ThrowUnexpectedChar();
				Source.ReadChar();
				result = (result << 4) + hexDigit;
			}
			return ((char)result);
		}

		private void ReadString(char endQuoteChar) {
			StringBuilder builder = new StringBuilder();
			Source.ReadChar();
			while (Source.CurChar != endQuoteChar) {
				if (Source.CurChar == -1)
					ThrowUnexpectedEndOfFile();
				else if (Source.CurChar != '\\') {
					builder.Append((char)Source.CurChar);
					Source.ReadChar();
				}
				else {
					Source.ReadChar();
					switch (Source.CurChar) {
						case '"':
						case '\\':
						case '/':
							builder.Append((char)Source.CurChar);
							Source.ReadChar();
							break;
						case 'b':
							builder.Append('\b');
							Source.ReadChar();
							break;
						case 'f':
							builder.Append('\f');
							Source.ReadChar();
							break;
						case 'n':
							builder.Append('\n');
							Source.ReadChar();
							break;
						case 'r':
							builder.Append('\r');
							Source.ReadChar();
							break;
						case 't':
							builder.Append('\t');
							Source.ReadChar();
							break;
						case 'v':
							builder.Append('\v');
							Source.ReadChar();
							break;
						case 'x':
							builder.Append(ReadEscapeSequence(2));
							break;
						case 'u':
							builder.Append(ReadEscapeSequence(4));
							break;
						default:
							// Учесть возможность слияния частей строкового литерала расположенных на разных строках
							if (Source.CurChar != '\n')
								builder.Append((char)Source.CurChar);
							Source.ReadChar();
							break;
					}
				}
			}
			CurToken.Type = TokenType.String;
			CurToken.Value = builder.ToString();
			Source.ReadChar();
		}

		/// <summary>
		/// Прочитать очередную лексему
		/// </summary>
		/// <param name="isRegexAllowed">Регулярные выражения допустимы? Влияет на обработку /</param>
		public void ReadToken(bool isRegexAllowed = false) {
			CurToken.SetUnknown();
			// Используется цикл так как могут встретится комментарии, которые необходимо пропускать
			while (!Source.IsEOF && CurToken.Type == TokenType.Unknown) {
				// Пропускаем пробельные символы
				while (char.IsWhiteSpace((char)Source.CurChar)) {
					// Запоминаем если встретили символ окончания строки (нужно для автоматической расстановки ;)
					// см. http://ecma-international.org/ecma-262/5.1/#sec-7.9
					if (Source.CurChar == '\n')
						CurToken.IsAfterLineTerminator = true;
					Source.ReadChar();
				}

				CurToken.StartPosition = new TokenPosition(Source.LineNo, Source.ColumnNo);

				if (IsIdentStartChar((char)Source.CurChar))
					ReadIdentOrKeyword();
				else {
					switch (Source.CurChar) {
						case '{':
							CurToken.Type = TokenType.LCurlyBrace;
							Source.ReadChar();
							break;
						case '}':
							CurToken.Type = TokenType.RCurlyBrace;
							Source.ReadChar();
							break;
						case '(':
							CurToken.Type = TokenType.LParenthesis;
							Source.ReadChar();
							break;
						case ')':
							CurToken.Type = TokenType.RParenthesis;
							Source.ReadChar();
							break;
						case '[':
							CurToken.Type = TokenType.LBracket;
							Source.ReadChar();
							break;
						case ']':
							CurToken.Type = TokenType.RBracket;
							Source.ReadChar();
							break;
						case '.':
							// Число также может начинаться с точки
							if (char.IsDigit((char)Source.PeekChar()))
								ReadIntegerOrFloatNumber();
							else {
								CurToken.Type = TokenType.Dot;
								Source.ReadChar();
							}
							break;
						case ';':
							CurToken.Type = TokenType.Semicolon;
							Source.ReadChar();
							break;
						case ',':
							CurToken.Type = TokenType.Comma;
							Source.ReadChar();
							break;
						case '<':
							Read_Lt_or_Lte_or_Shl_or_ShlAssign();
							break;
						case '>':
							Read_Gt_or_Gte_or_ShrS_or_ShrSAssign_or_ShrU_or_ShrUAssign();
							break;
						case '=':
							Read_Eq_or_Assign_or_StrictEq();
							break;
						case '+':
							Read_Plus_or_PlusAssign_or_Inc();
							break;
						case '-':
							Read_Minus_or_MinusAssign_or_Dec();
							break;
						case '*':
							Read_Star_or_StarAssign();
							break;
						case '/':
							TryRead_Slash_or_SlashAssign(isRegexAllowed);
							break;
						case '%':
							Read_Mod_or_ModAssign();
							break;
						case '~':
							CurToken.Type = TokenType.BitNot;
							Source.ReadChar();
							break;
						case '&':
							Read_BitAnd_or_BitAndAssign_or_And();
							break;
						case '|':
							Read_BitOr_or_BitOrAssign_or_Or();
							break;
						case '^':
							Read_BitXor_or_BitXorAssign();
							break;
						case '!':
							Read_Not_or_Neq_or_StrictNeq();
							break;
						case '?':
							CurToken.Type = TokenType.QuestionMark;
							Source.ReadChar();
							break;
						case ':':
							CurToken.Type = TokenType.Colon;
							Source.ReadChar();
							break;

						case '0':
							Source.ReadChar();
							if (Source.CurChar == 'x' || Source.CurChar == 'X')
								ReadHexIntegerNumber();
							else
								ReadIntegerOrFloatNumber();
							break;
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							ReadIntegerOrFloatNumber();
							break;

						case '\'':
							ReadString('\'');
							break;
						case '\"':
							ReadString('\"');
							break;

						default:
							if (Source.CurChar != -1)
								ThrowUnexpectedChar();
							break;
					}
				}
			}
		}

		public CharStream Source { get; private set; }
		public Token CurToken { get; private set; }
	}
}
