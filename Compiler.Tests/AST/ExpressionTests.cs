using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Expressions;

namespace YaJS.Compiler.Tests.AST {
	[TestClass]
	public sealed class ExpressionTests {
		[TestMethod]
		public void Integer_Simple() {
			const int expected = 10;
			var actual = Expression.Integer(expected.ToString(CultureInfo.InvariantCulture)) as IntegerLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.Value, expected);
		}

		[TestMethod]
		public void Integer_ReturnFloat() {
			const long expected = (long)int.MaxValue + 1L;
			var actual = Expression.Integer(expected.ToString(CultureInfo.InvariantCulture)) as FloatLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual((long)actual.Value, expected);
		}

		private static void HexInteger_Simple(string format) {
			const int expected = 10;
			var actual = Expression.HexInteger(expected.ToString(format, CultureInfo.InvariantCulture)) as IntegerLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.Value, expected);
		}

		[TestMethod]
		public void HexInteger_SimpleLower() {
			HexInteger_Simple("x");
		}

		[TestMethod]
		public void HexInteger_SimpleUpper() {
			HexInteger_Simple("X");
		}

		private static void HexInteger_ReturnFloat(string format) {
			const long expected = (long)int.MaxValue + 1L;
			var actual = Expression.HexInteger(expected.ToString(format, CultureInfo.InvariantCulture)) as FloatLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual((long)actual.Value, expected);
		}

		[TestMethod]
		public void HexInteger_ReturnFloatLower() {
			HexInteger_ReturnFloat("x");
		}

		[TestMethod]
		public void HexInteger_ReturnFloatUpper() {
			HexInteger_ReturnFloat("X");
		}
	}
}
