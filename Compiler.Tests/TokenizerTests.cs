using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.Exceptions;

namespace YaJS.Compiler.Tests {
	[TestClass]
	public class TokenizerTests {
		private static Token RunTokenizer(string source) {
			var tokenizer = new Tokenizer(new StringReader(source));
			tokenizer.ReadToken();
			return (tokenizer.Lookahead);
		}

		private static void RunTokenizer(string source, TokenType expectedType) {
			var token = RunTokenizer(source);
			Assert.AreEqual(token.Type, expectedType, source);
			Assert.IsTrue(string.CompareOrdinal(token.Value, source) == 0, source);
		}

		[AssertionMethod]
		private static void RunTokenizer(string source, TokenType expectedType, string expectedValue) {
			var token = RunTokenizer(source);
			Assert.AreEqual(token.Type, expectedType, source);
			Assert.IsTrue(string.CompareOrdinal(token.Value, expectedValue) == 0, source);
		}

		[TestMethod]
		public void Empty() {
			var token = RunTokenizer("");
			Assert.AreEqual(token.Type, TokenType.Unknown);
		}

		[TestMethod]
		public void OnlyWhiteSpaces() {
			var token = RunTokenizer(" ");
			Assert.AreEqual(token.Type, TokenType.Unknown);
		}

		[TestMethod]
		public void WhiteSpacesBeforeIdent() {
			var token = RunTokenizer(" \n a");
			Assert.AreEqual(token.Type, TokenType.Ident);
			Assert.IsTrue(token.IsAfterLineTerminator);
		}

		[TestMethod]
		public void SingleLineComment_Simple() {
			var token = RunTokenizer("// comment");
			Assert.IsTrue(token.Type == TokenType.Unknown);
		}

		[TestMethod]
		public void MultiLineComment_Simple() {
			var token = RunTokenizer("/* comment * comment */");
			Assert.IsTrue(token.Type == TokenType.Unknown);
		}

		[TestMethod]
		public void MultiLineComment_IncludeLineTerminator() {
			var token = RunTokenizer("/* line1\nline2 */");
			Assert.IsTrue(token.Type == TokenType.Unknown);
			Assert.IsTrue(token.IsAfterLineTerminator);
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedEndOfFileException))]
		public void MultiLineComment_Unterminated() {
			RunTokenizer("/* line1\nline2");
		}

		[TestMethod]
		public void Ident_Simple() {
			RunTokenizer("a1", TokenType.Ident);
			RunTokenizer("$a1", TokenType.Ident);
			RunTokenizer("_a1$1", TokenType.Ident);
		}

		private static Dictionary<string, TokenType> GetKeywords() {
			return (new Dictionary<string, TokenType> {
				{"break", TokenType.Break},
				{"case", TokenType.Case},
				{"catch", TokenType.Catch},
				{"continue", TokenType.Continue},
				{"debugger", TokenType.Debugger},
				{"default", TokenType.Default},
				{"delete", TokenType.Delete},
				{"do", TokenType.Do},
				{"else", TokenType.Else},
				{"false", TokenType.False},
				{"finally", TokenType.Finally},
				{"for", TokenType.For},
				{"function", TokenType.Function},
				{"if", TokenType.If},
				{"in", TokenType.In},
				{"instanceof", TokenType.InstanceOf},
				{"null", TokenType.Null},
				{"new", TokenType.New},
				{"return", TokenType.Return},
				{"switch", TokenType.Switch},
				{"this", TokenType.This},
				{"throw", TokenType.Throw},
				{"true", TokenType.True},
				{"try", TokenType.Try},
				{"typeof", TokenType.Typeof},
				{"var", TokenType.Var},
				{"void", TokenType.Void},
				{"while", TokenType.While},
				{"with", TokenType.With},
				{"class", TokenType.Class},
				{"enum", TokenType.Enum},
				{"extends", TokenType.Extends},
				{"super", TokenType.Super},
				{"const", TokenType.Const},
				{"export", TokenType.Export},
				{"import", TokenType.Import},
				{"implements", TokenType.Implements},
				{"let", TokenType.Let},
				{"private", TokenType.Private},
				{"public", TokenType.Public},
				{"yield", TokenType.Yield},
				{"interface", TokenType.Interface},
				{"package", TokenType.Package},
				{"protected", TokenType.Protected},
				{"static", TokenType.Static}
			});
		}

		[TestMethod]
		public void Keywords_Simple() {
			foreach (var keyword in GetKeywords())
				Assert.IsTrue(RunTokenizer(keyword.Key).Type == keyword.Value, keyword.Key);
		}

		private static Dictionary<string, TokenType> GetPunctuators() {
			return (new Dictionary<string, TokenType> {
				{"{", TokenType.LCurlyBrace},
				{"}", TokenType.RCurlyBrace},
				{"(", TokenType.LParenthesis},
				{")", TokenType.RParenthesis},
				{"[", TokenType.LBracket},
				{"]", TokenType.RBracket},
				{",", TokenType.Comma},
				{".", TokenType.Dot},
				{";", TokenType.Semicolon},
				{"<", TokenType.Lt},
				{">", TokenType.Gt},
				{"<=", TokenType.Lte},
				{">=", TokenType.Gte},
				{"==", TokenType.Eq},
				{"!=", TokenType.Neq},
				{"===", TokenType.StrictEq},
				{"!==", TokenType.StrictNeq},
				{"+", TokenType.Plus},
				{"-", TokenType.Minus},
				{"*", TokenType.Star},
				{"/", TokenType.Slash},
				{"%", TokenType.Mod},
				{"++", TokenType.Inc},
				{"--", TokenType.Dec},
				{"<<", TokenType.Shl},
				{">>", TokenType.ShrS},
				{">>>", TokenType.ShrU},
				{"&", TokenType.BitAnd},
				{"|", TokenType.BitOr},
				{"^", TokenType.BitXor},
				{"!", TokenType.Not},
				{"~", TokenType.BitNot},
				{"&&", TokenType.And},
				{"||", TokenType.Or},
				{"?", TokenType.QuestionMark},
				{":", TokenType.Colon},
				{"=", TokenType.Assign},
				{"+=", TokenType.PlusAssign},
				{"-=", TokenType.MinusAssign},
				{"*=", TokenType.StarAssign},
				{"/=", TokenType.SlashAssign},
				{"%=", TokenType.ModAssign},
				{"<<=", TokenType.ShlAssign},
				{">>=", TokenType.ShrSAssign},
				{">>>=", TokenType.ShrUAssign},
				{"&=", TokenType.BitAndAssign},
				{"|=", TokenType.BitOrAssign},
				{"^=", TokenType.BitXorAssign}
			});
		}

		[TestMethod]
		public void Punctuators_Simple() {
			foreach (var punctuator in GetPunctuators())
				Assert.IsTrue(RunTokenizer(punctuator.Key).Type == punctuator.Value, punctuator.Key);
		}

		[TestMethod]
		public void Integer_Zero() {
			RunTokenizer("0", TokenType.Integer);
		}

		[TestMethod]
		public void Integer_Simple() {
			RunTokenizer("123", TokenType.Integer);
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void Integer_NextImmediateLetter() {
			RunTokenizer("123a");
		}

		[TestMethod]
		public void HexInteger_Simple() {
			RunTokenizer("0x123ac", TokenType.HexInteger, "123ac");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void HexInteger_NextImmediateLetter() {
			RunTokenizer("0x123acz");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void HexInteger_InvalidChar() {
			RunTokenizer("0xz");
		}

		[TestMethod]
		public void Float_Simple() {
			RunTokenizer("145.145", TokenType.Float);
			RunTokenizer(".145", TokenType.Float, "0.145");

			RunTokenizer("145e-145", TokenType.Float);
			RunTokenizer("145e145", TokenType.Float);
			RunTokenizer("145e+145", TokenType.Float);

			RunTokenizer(".145e-145", TokenType.Float, "0.145e-145");
			RunTokenizer(".145e145", TokenType.Float, "0.145e145");
			RunTokenizer(".145e+145", TokenType.Float, "0.145e+145");

			RunTokenizer("145.145e-145", TokenType.Float);
			RunTokenizer("145.145e145", TokenType.Float);
			RunTokenizer("145.145e+145", TokenType.Float);
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void Float_NextImmediateLetter() {
			RunTokenizer("145.145a");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void Float_InvalidCharAfterDot() {
			RunTokenizer("145.a");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void Float_InvalidCharAfterE() {
			RunTokenizer("145.0ez");
		}

		private static Dictionary<string, string> GetCharEscapeSequences() {
			return (new Dictionary<string, string> {
				{"'\\''", "\'"},
				{"'\\\"'", "\""},
				{"'\\\\'", "\\"},
				{"'\\b'", "\b"},
				{"'\\f'", "\f"},
				{"'\\n'", "\n"},
				{"'\\r'", "\r"},
				{"'\\t'", "\t"},
				{"'\\v'", "\v"}
			});
		}

		[TestMethod]
		public void String_Simple() {
			RunTokenizer("'abc'", TokenType.String, "abc");
			RunTokenizer("\"abc\"", TokenType.String, "abc");
		}

		[TestMethod]
		public void String_CharEscapeSequences() {
			foreach (var escapeSequence in GetCharEscapeSequences())
				RunTokenizer(escapeSequence.Key, TokenType.String, escapeSequence.Value);
		}

		[TestMethod]
		public void String_IgnoreInvalidEscapeSequences() {
			RunTokenizer("'ab\\zc'", TokenType.String, "abzc");
		}

		[TestMethod]
		public void String_LineContinuation() {
			RunTokenizer("'abc\\\nd'", TokenType.String, "abcd");
		}

		[TestMethod]
		public void String_HexEscapeSequence_Lower() {
			RunTokenizer("'\\x0a'", TokenType.String, "\n");
		}

		[TestMethod]
		public void String_HexEscapeSequence_Upper() {
			RunTokenizer("'\\x0F'", TokenType.String, "\x0F");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void String_InvalidHexEscapeSequence() {
			RunTokenizer("'\\xz'");
		}

		[TestMethod]
		public void String_UnicodeEscapeSequence() {
			RunTokenizer("'\\u000a'", TokenType.String, "\n");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void String_UnicodeHexEscapeSequence() {
			RunTokenizer("'\\uz'");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedEndOfFileException))]
		public void String_Unterminated() {
			RunTokenizer("'abc");
		}

		[TestMethod]
		[ExpectedException(typeof (UnexpectedCharException))]
		public void InvalidChar() {
			RunTokenizer("#");
		}
	}
}
