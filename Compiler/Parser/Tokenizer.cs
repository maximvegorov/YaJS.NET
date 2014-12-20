using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace YaJS.Compiler.Parser {
	using Compiler.Exceptions;
	using Compiler.Helpers;

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
			return (char.IsLetterOrDigit(c) || c == '$' || c == '_');
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
		}

		private void SkipMultiLineComment() {
		}

		private void Read_Slash_or_SlashAssign(bool isRegexAllowed) {
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
			else if (Source.CurChar == '&') {
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

		private void ReadNumber() {
		}

		private void ReadString(char quoteChar) {
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

		public void ReadToken(bool isRegexAllowed = false) {
			CurToken.SetUnknown();
			while (!Source.IsEOF && CurToken.Type == TokenType.Unknown) {
				// Пропускаем пробельные символы
				while (char.IsWhiteSpace((char)Source.CurChar)) {
					// Запоминаем если встретили символ окончания строки (нужно для автоматической расстановки ;)
					if (Source.CurChar == '\n')
						CurToken.IsAfterLineTerminator = true;
					Source.ReadChar();
				}
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
							CurToken.Type = TokenType.Dot;
							Source.ReadChar();
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
							Read_Slash_or_SlashAssign(isRegexAllowed);
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
							CurToken.Type = TokenType.Not;
							Source.ReadChar();
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
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							ReadNumber();
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
