using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.AST;

namespace YaJS.Compiler.Tests {
	[TestClass]
	public partial class ParserTests {
		private static Expression ParseExpression(string source) {
			var tokenizer = new Tokenizer(new StringReader(source));
			var parser = new Parser(tokenizer);
			return (parser.ParseExpression());
		}

		private static Function ParseFunction(string source) {
			var tokenizer = new Tokenizer(new StringReader(source));
			var parser = new Parser(tokenizer);
			return (parser.ParseFunction("global", Enumerable.Empty<string>()));
		}
	}
}
