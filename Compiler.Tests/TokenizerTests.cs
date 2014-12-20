using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YaJS.Compiler.Tests {
	using Compiler.Parser;

	[TestClass]
	public class TokenizerTests {
		private static Token RunTokenizer(string source) {
			var tokenizer = new Tokenizer(new StringReader(source));
			tokenizer.ReadToken();
			return (tokenizer.CurToken);
		}

		private static void RunIdentSimple(string ident) {
			var token = RunTokenizer(ident);
			Assert.IsTrue(token.Type == TokenType.Ident);
			Assert.IsTrue(token.Value == ident);
		}

		[TestMethod]
		public void Ident_Simple() {
			RunIdentSimple("a1");
			RunIdentSimple("$a1");
			RunIdentSimple("_a1$1");
		}

		private static Dictionary<string, TokenType> GetKeywords() {
			return (new Dictionary<string, TokenType>() {
				{ "break", TokenType.Break },
				{ "case", TokenType.Case },
				{ "catch", TokenType.Catch },
				{ "continue", TokenType.Continue },
				{ "debugger", TokenType.Debugger },
				{ "default", TokenType.Default },
				{ "delete", TokenType.Delete },
				{ "do", TokenType.Do },
				{ "else", TokenType.Else },
				{ "false", TokenType.False },
				{ "finally", TokenType.Finally },
				{ "for", TokenType.For },
				{ "function", TokenType.Function },
				{ "if", TokenType.If },
				{ "in", TokenType.In },
				{ "instanceof", TokenType.InstanceOf },
				{ "null", TokenType.Null },
				{ "new", TokenType.New },
				{ "return", TokenType.Return },
				{ "switch", TokenType.Switch },
				{ "this", TokenType.This },
				{ "throw", TokenType.Throw },
				{ "true", TokenType.True },
				{ "try", TokenType.Try },
				{ "typeof", TokenType.Typeof },
				{ "var", TokenType.Var },
				{ "void", TokenType.Void },
				{ "while", TokenType.While },
				{ "with", TokenType.With },
				{ "class", TokenType.Class },
				{ "enum", TokenType.Enum },
				{ "extends", TokenType.Extends },
				{ "super", TokenType.Super },
				{ "const", TokenType.Const },
				{ "export", TokenType.Export },
				{ "import", TokenType.Import },
				{ "implements", TokenType.Implements },
				{ "let", TokenType.Let },
				{ "private", TokenType.Private },
				{ "public", TokenType.Public },
				{ "yield", TokenType.Yield },
				{ "interface", TokenType.Interface },
				{ "package", TokenType.Package },
				{ "protected", TokenType.Protected },
				{ "static", TokenType.Static },
			});
		}

		[TestMethod]
		public void Keyword_Simple() {
			foreach (var keyword in GetKeywords()) {
				Assert.IsTrue(RunTokenizer(keyword.Key).Type == keyword.Value);
			}
		}
	}
}
