using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YaJS.Compiler.Tests {
	using YaJS.Compiler.Parser;

	[TestClass]
	public class CharStreamTests {
		private static CharStream Create(string source) {
			return (new CharStream(new StringReader(source)));
		}

		[TestMethod]
		public void LineTerminator() {
			var stream = Create("\n\ra\u2028");
			Assert.IsTrue(stream.CurChar == '\n' && stream.LineNo == 2 && stream.ColumnNo == 0);
			stream.ReadChar();
			Assert.IsTrue(stream.CurChar == 'a' && stream.LineNo == 2 && stream.ColumnNo == 1);
			stream.ReadChar();
			Assert.IsTrue(stream.CurChar == '\n' && stream.LineNo == 3 && stream.ColumnNo == 0);
		}
	}
}
