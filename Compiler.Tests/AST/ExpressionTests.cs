using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.AST.Expressions;

namespace YaJS.Compiler.AST.Tests {
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
	}
}
