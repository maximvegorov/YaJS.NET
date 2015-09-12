using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.AST;

namespace YaJS.Compiler.Tests {
	[TestClass]
	public sealed class ParserTests {
		private static Function ParseFunction(string source) {
			using (var reader = new StringReader(source)) {
				var tokenizer = new Tokenizer(reader);
				var parser = new Parser(tokenizer);
				return (parser.ParseFunction("global", Enumerable.Empty<string>()));
			}
		}

		[TestMethod]
		public void ParseDirectives_Empty() {
			var actual = ParseFunction("");
			Assert.AreEqual(actual.Directives.Count, 0);
		}

		[TestMethod]
		public void ParseDirectives_SingleWithSemicolon() {
			const string expected = "a";
			var actual = ParseFunction(string.Format("'{0}';", expected));
			Assert.AreEqual(actual.Directives.Count, 1);
			Assert.IsTrue(actual.Directives.Contains(expected));
		}

		[TestMethod]
		public void ParseDirectives_SingleWithoutSemicolon() {
			const string expected = "a";
			var actual = ParseFunction(string.Format("'{0}'", expected));
			Assert.AreEqual(actual.Directives.Count, 1);
			Assert.IsTrue(actual.Directives.Contains(expected));
		}

		[TestMethod]
		public void ParseDirectives_MultipleMixed() {
			const string directive1 = "a";
			const string directive2 = "b";
			const string directive3 = "c";
			var actual = ParseFunction(
				string.Format("'{0}'\n'{1}';\n'{2}'", directive1, directive2, directive3));
			Assert.AreEqual(actual.Directives.Count, 3);
			Assert.IsTrue(actual.Directives.Contains(directive1));
			Assert.IsTrue(actual.Directives.Contains(directive2));
			Assert.IsTrue(actual.Directives.Contains(directive3));
		}

		[TestMethod]
		public void ParseDirectives_Duplicates() {
			const string directive1 = "a";
			const string directive2 = "b";
			var actual = ParseFunction(
				string.Format("'{0}'\n'{1}';\n'{2}'", directive1, directive2, directive1));
			Assert.AreEqual(actual.Directives.Count, 2);
			Assert.IsTrue(actual.Directives.Contains(directive1));
			Assert.IsTrue(actual.Directives.Contains(directive2));
		}
	}
}
