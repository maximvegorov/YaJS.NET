using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace YaJS.Compiler {
	using YaJS.Compiler.Exceptions;

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

			Keywords.Add("undefined", TokenType.Undefined);
			Keywords.Add("eval", TokenType.Eval);
			Keywords.Add("arguments", TokenType.Arguments);
		}

		public CharStream _input;

		public Tokenizer(TextReader reader) {
			Contract.Requires<ArgumentNullException>(reader != null, "reader");
			_input = new CharStream(reader);
			Lookahead = new Token();
		}

		private static bool IsIdentStartChar(char c) {
			return (char.IsLetter(c) || c == '$' || c == '_');
		}

		private void ReadIdentOrKeyword() {
			StringBuilder builder = new StringBuilder();
			builder.Append((char)_input.CurChar);
			_input.ReadChar();
			while (char.IsLetterOrDigit((char)_input.CurChar) || _input.CurChar == '$' || _input.CurChar == '_' || _input.CurChar == '\u200C' || _input.CurChar == '\u200D') {
				builder.Append((char)_input.CurChar);
				_input.ReadChar();
			}
			var value = builder.ToString();
			TokenType keyword;
			if (Keywords.TryGetValue(value, out keyword))
				Lookahead.Type = keyword;
			else {
				Lookahead.Type = TokenType.Ident;
				Lookahead.Value = value;
			}
		}

		private void Read_Lt_or_Lte_or_Shl_or_ShlAssign() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.Lte;
				_input.ReadChar();
			}
			else if (_input.CurChar == '<') {
				_input.ReadChar();
				if (_input.CurChar == '=') {
					Lookahead.Type = TokenType.ShlAssign;
					_input.ReadChar();
				}
				else
					Lookahead.Type = TokenType.Shl;
			}
			else
				Lookahead.Type = TokenType.Lt;
		}

		private void Read_Gt_or_Gte_or_ShrS_or_ShrSAssign_or_ShrU_or_ShrUAssign() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.Gte;
				_input.ReadChar();
			}
			else if (_input.CurChar == '>') {
				_input.ReadChar();
				if (_input.CurChar == '=') {
					Lookahead.Type = TokenType.ShrSAssign;
					_input.ReadChar();
				}
				else if (_input.CurChar == '>') {
					_input.ReadChar();
					if (_input.CurChar == '=') {
						Lookahead.Type = TokenType.ShrUAssign;
						_input.ReadChar();
					}
					else
						Lookahead.Type = TokenType.ShrU;
				}
				else
					Lookahead.Type = TokenType.ShrS;
			}
			else
				Lookahead.Type = TokenType.Gt;
		}

		private void Read_Eq_or_Assign_or_StrictEq() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				_input.ReadChar();
				if (_input.CurChar == '=') {
					Lookahead.Type = TokenType.StrictEq;
					_input.ReadChar();
				}
				else
					Lookahead.Type = TokenType.Eq;
			}
			else
				Lookahead.Type = TokenType.Assign;
		}

		private void Read_Plus_or_PlusAssign_or_Inc() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.PlusAssign;
				_input.ReadChar();
			}
			else if (_input.CurChar == '+') {
				Lookahead.Type = TokenType.Inc;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.Plus;
		}

		private void Read_Minus_or_MinusAssign_or_Dec() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.MinusAssign;
				_input.ReadChar();
			}
			else if (_input.CurChar == '-') {
				Lookahead.Type = TokenType.Dec;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.Minus;
		}

		private void Read_Star_or_StarAssign() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.StarAssign;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.Star;
		}

		private void SkipSingleLineComment() {
			do {
				_input.ReadChar();
			}
			while (_input.CurChar != '\n' && _input.CurChar != -1);
		}

		private void ThrowUnexpectedEndOfFile() {
			throw new UnexpectedEndOfFileException(
				Messages.Error(
					_input.LineNo,
					_input.ColumnNo,
					"Unexpected end of file."
				)
			);
		}

		private void SkipMultiLineComment() {
			for(;;) {
				_input.ReadChar();
				switch (_input.CurChar) {
					case -1:
						ThrowUnexpectedEndOfFile();
						break;
					case '\n':
						// Если многострочный комментарий содержит символ окончания строки,
						// то считаем что следующей лексеме предшествует символ окончания строки
						// см. http://ecma-international.org/ecma-262/5.1/#sec-7.4
						Lookahead.IsAfterLineTerminator = true;
						break;
					case '*':
						_input.ReadChar();
						if (_input.CurChar == '/') {
							_input.ReadChar();
							return;
						}
						break;
				}
			}
		}

		private void TryRead_Slash_or_SlashAssign(bool isRegexAllowed) {
			_input.ReadChar();
			if (!isRegexAllowed && _input.CurChar == '=') {
				Lookahead.Type = TokenType.SlashAssign;
				_input.ReadChar();
			}
			else if (_input.CurChar == '/')
				SkipSingleLineComment();
			else if (_input.CurChar == '*')
				SkipMultiLineComment();
			else
				Lookahead.Type = TokenType.Slash;
		}

		private void Read_Mod_or_ModAssign() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.ModAssign;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.Mod;
		}

		private void Read_BitAnd_or_BitAndAssign_or_And() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.BitAndAssign;
				_input.ReadChar();
			}
			else if (_input.CurChar == '&') {
				Lookahead.Type = TokenType.And;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.BitAnd;
		}

		private void Read_BitOr_or_BitOrAssign_or_Or() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.BitOrAssign;
				_input.ReadChar();
			}
			else if (_input.CurChar == '|') {
				Lookahead.Type = TokenType.Or;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.BitOr;
		}

		private void Read_BitXor_or_BitXorAssign() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				Lookahead.Type = TokenType.BitXorAssign;
				_input.ReadChar();
			}
			else
				Lookahead.Type = TokenType.BitXor;
		}

		private void Read_Not_or_Neq_or_StrictNeq() {
			_input.ReadChar();
			if (_input.CurChar == '=') {
				_input.ReadChar();
				if (_input.CurChar == '=') {
					Lookahead.Type = TokenType.StrictNeq;
					_input.ReadChar();
				}
				else
					Lookahead.Type = TokenType.Neq;
			}
			else
				Lookahead.Type = TokenType.Not;
		}

		private void ThrowUnexpectedChar() {
			throw new UnexpectedCharException(
				Messages.Error(
					_input.LineNo,
					_input.ColumnNo,
					string.Format("Unexpected char \"{0}\".", _input.CurChar.ToString())
				)
			);
		}

		private void ReadIntegerOrFloatNumber() {
			Lookahead.Type = TokenType.Integer;
			StringBuilder builder = new StringBuilder();
			while (char.IsDigit((char)_input.CurChar)) {
				builder.Append((char)_input.CurChar);
				_input.ReadChar();
			}
			// Ноль может быть удален при попытке распознать HexInteger
			if (builder.Length == 0)
				builder.Append('0');
			if (_input.CurChar == '.') {
				Lookahead.Type = TokenType.Float;
				builder.Append((char)_input.CurChar);
				_input.ReadChar();
				if (!char.IsDigit((char)_input.CurChar))
					ThrowUnexpectedChar();
				do {
					builder.Append((char)_input.CurChar);
					_input.ReadChar();
				} while (char.IsDigit((char)_input.CurChar));
			}
			if (_input.CurChar == 'e' || _input.CurChar == 'E') {
				Lookahead.Type = TokenType.Float;
				builder.Append((char)_input.CurChar);
				_input.ReadChar();
				if (_input.CurChar == '-' || _input.CurChar == '+') {
					builder.Append((char)_input.CurChar);
					_input.ReadChar();
				}
				if (!char.IsDigit((char)_input.CurChar))
					ThrowUnexpectedChar();
				do {
					builder.Append((char)_input.CurChar);
					_input.ReadChar();
				} while (char.IsDigit((char)_input.CurChar));
			}
			// Число и идентификатор должны быть разделены хотя бы одним символом
			// см. http://ecma-international.org/ecma-262/5.1/#sec-7.8.3
			if (IsIdentStartChar((char)_input.CurChar))
				ThrowUnexpectedChar();
			Lookahead.Value = builder.ToString();
		}

		private static bool IsHexDigit(char c) {
			return (char.IsDigit(c) || ('a' <= c && c <= 'f') || ('A' <= c && c <= 'F'));
		}

		private void ReadHexIntegerNumber() {
			// Пропустить x или X
			_input.ReadChar();
			if (!IsHexDigit((char)_input.CurChar))
				ThrowUnexpectedChar();
			StringBuilder builder = new StringBuilder();
			do {
				builder.Append((char)_input.CurChar);
				_input.ReadChar();
			}
			while (IsHexDigit((char)_input.CurChar));
			// Число и идентификатор должны быть разделены хотя бы одним символом
			// см. http://ecma-international.org/ecma-262/5.1/#sec-7.8.3
			if (IsIdentStartChar((char)_input.CurChar))
				ThrowUnexpectedChar();
			Lookahead.Type = TokenType.HexInteger;
			Lookahead.Value = builder.ToString();
		}

		private char ReadEscapeSequence(int length) {
			_input.ReadChar();
			int result = 0;
			for (int i = 0; i < length; i++) {
				int hexDigit = 0;
				if ('0' <= _input.CurChar && _input.CurChar <= '9')
					hexDigit = _input.CurChar - '0';
				else if ('a' <= _input.CurChar && _input.CurChar <= 'f')
					hexDigit = _input.CurChar - 'a' + 10;
				else if ('A' <= _input.CurChar && _input.CurChar <= 'F')
					hexDigit = _input.CurChar - 'A' + 10;
				else
					ThrowUnexpectedChar();
				_input.ReadChar();
				result = (result << 4) + hexDigit;
			}
			return ((char)result);
		}

		private void ReadString(char endQuoteChar) {
			StringBuilder builder = new StringBuilder();
			_input.ReadChar();
			while (_input.CurChar != endQuoteChar) {
				if (_input.CurChar == -1)
					ThrowUnexpectedEndOfFile();
				else if (_input.CurChar != '\\') {
					builder.Append((char)_input.CurChar);
					_input.ReadChar();
				}
				else {
					_input.ReadChar();
					switch (_input.CurChar) {
						case '"':
						case '\\':
						case '/':
							builder.Append((char)_input.CurChar);
							_input.ReadChar();
							break;
						case 'b':
							builder.Append('\b');
							_input.ReadChar();
							break;
						case 'f':
							builder.Append('\f');
							_input.ReadChar();
							break;
						case 'n':
							builder.Append('\n');
							_input.ReadChar();
							break;
						case 'r':
							builder.Append('\r');
							_input.ReadChar();
							break;
						case 't':
							builder.Append('\t');
							_input.ReadChar();
							break;
						case 'v':
							builder.Append('\v');
							_input.ReadChar();
							break;
						case 'x':
							builder.Append(ReadEscapeSequence(2));
							break;
						case 'u':
							builder.Append(ReadEscapeSequence(4));
							break;
						default:
							// Учесть возможность слияния частей строкового литерала расположенных на разных строках
							if (_input.CurChar != '\n')
								builder.Append((char)_input.CurChar);
							_input.ReadChar();
							break;
					}
				}
			}
			Lookahead.Type = TokenType.String;
			Lookahead.Value = builder.ToString();
			_input.ReadChar();
		}

		/// <summary>
		/// Прочитать очередную лексему
		/// </summary>
		/// <param name="isRegexAllowed">Регулярные выражения допустимы? Влияет на обработку /</param>
		public Token ReadToken(bool isRegexAllowed = false) {
			Lookahead.SetUnknown();
			// Используется цикл так как могут встретится комментарии, которые необходимо пропускать
			while (!_input.IsEOF && Lookahead.Type == TokenType.Unknown) {
				// Пропускаем пробельные символы
				while (char.IsWhiteSpace((char)_input.CurChar)) {
					// Запоминаем если встретили символ окончания строки (нужно для автоматической расстановки ;)
					// см. http://ecma-international.org/ecma-262/5.1/#sec-7.9
					if (_input.CurChar == '\n')
						Lookahead.IsAfterLineTerminator = true;
					_input.ReadChar();
				}

				Lookahead.StartPosition = new TokenPosition(_input.LineNo, _input.ColumnNo);

				if (IsIdentStartChar((char)_input.CurChar))
					ReadIdentOrKeyword();
				else {
					switch (_input.CurChar) {
						case '{':
							Lookahead.Type = TokenType.LCurlyBrace;
							_input.ReadChar();
							break;
						case '}':
							Lookahead.Type = TokenType.RCurlyBrace;
							_input.ReadChar();
							break;
						case '(':
							Lookahead.Type = TokenType.LParenthesis;
							_input.ReadChar();
							break;
						case ')':
							Lookahead.Type = TokenType.RParenthesis;
							_input.ReadChar();
							break;
						case '[':
							Lookahead.Type = TokenType.LBracket;
							_input.ReadChar();
							break;
						case ']':
							Lookahead.Type = TokenType.RBracket;
							_input.ReadChar();
							break;
						case '.':
							// Число также может начинаться с точки
							if (char.IsDigit((char)_input.PeekChar()))
								ReadIntegerOrFloatNumber();
							else {
								Lookahead.Type = TokenType.Dot;
								_input.ReadChar();
							}
							break;
						case ';':
							Lookahead.Type = TokenType.Semicolon;
							_input.ReadChar();
							break;
						case ',':
							Lookahead.Type = TokenType.Comma;
							_input.ReadChar();
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
							Lookahead.Type = TokenType.BitNot;
							_input.ReadChar();
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
							Lookahead.Type = TokenType.QuestionMark;
							_input.ReadChar();
							break;
						case ':':
							Lookahead.Type = TokenType.Colon;
							_input.ReadChar();
							break;

						case '0':
							_input.ReadChar();
							if (_input.CurChar == 'x' || _input.CurChar == 'X')
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
							if (_input.CurChar != -1)
								ThrowUnexpectedChar();
							break;
					}
				}
			}
			return (Lookahead);
		}

		public Token Lookahead { get; private set; }
	}
}
