using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Tests.Values {
	[TestClass]
	public sealed class JSNumberValueTests {
		[TestMethod]
		public void ParseNumber_Infinity_Exact_Ok() {
			var actual = JSNumberValue.ParseNumber("Infinity");
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsPositiveInfinity(actual.CastToFloat()));
		}

		[TestMethod]
		public void ParseNumber_PosInfinity_Exact_Ok() {
			var actual = JSNumberValue.ParseNumber("+Infinity");
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsPositiveInfinity(actual.CastToFloat()));
		}

		[TestMethod]
		public void ParseNumber_NegInfinity_Exact_Ok() {
			var actual = JSNumberValue.ParseNumber("-Infinity");
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsNegativeInfinity(actual.CastToFloat()));
		}

		[TestMethod]
		public void ParseNumber_Infinity_LeadingSpaces_And_FollowingSpaces_Ok() {
			var actual = JSNumberValue.ParseNumber("  Infinity  ");
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsPositiveInfinity(actual.CastToFloat()));
		}

		[TestMethod]
		public void ParseNumber_Infinity_Simple_Fail() {
			var actual = JSNumberValue.ParseNumber("  Infiniti  ");
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsNaN(actual.CastToFloat()));
		}

		[TestMethod]
		public void ParseNumber_Infinity_FollowingNotOnlySpaces_Fail() {
			var actual = JSNumberValue.ParseNumber("  Infinity  1");
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsNaN(actual.CastToFloat()));
		}

		private static void ParseNumber_Integer_Ok(string prefix, string format, string suffix) {
			const int expected = 1000;
			var actual = JSNumberValue.ParseNumber(prefix + expected.ToString(format, CultureInfo.InvariantCulture) + suffix);
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Type == JSValueType.Integer && actual.CastToInteger() == expected);
		}

		private static void ParseNumber_Integer_Fail(string value) {
			var actual = JSNumberValue.ParseNumber(value);
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsNaN(actual.CastToFloat()));
		}

		private static void ParseNumber_Integer_ReturnFloat_Ok(string prefix, string format, string suffix) {
			const long expected = (long)int.MaxValue + 1L;
			var actual = JSNumberValue.ParseNumber(prefix + expected.ToString(format, CultureInfo.InvariantCulture) + suffix);
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Type == JSValueType.Float && Math.Abs(actual.CastToFloat() - expected) < 1e-9);
		}

		[TestMethod]
		public void ParseNumber_Integer() {
			ParseNumber_Integer_Ok(string.Empty, "d", string.Empty);
			ParseNumber_Integer_Ok(string.Empty, "d", " ");
			ParseNumber_Integer_ReturnFloat_Ok(string.Empty, "d", string.Empty);
			ParseNumber_Integer_ReturnFloat_Ok(string.Empty, "d", " ");
		}

		[TestMethod]
		public void ParseNumber_HexInteger() {
			ParseNumber_Integer_Ok("0x", "x", string.Empty);
			ParseNumber_Integer_Ok("0x", "X", string.Empty);
			ParseNumber_Integer_Ok("0x", "x", " ");
			ParseNumber_Integer_ReturnFloat_Ok("0x", "x", string.Empty);
			ParseNumber_Integer_ReturnFloat_Ok("0x", "X", string.Empty);
			ParseNumber_Integer_ReturnFloat_Ok("0x", "x", " ");
			ParseNumber_Integer_Fail("0xr");
			ParseNumber_Integer_Fail("0Xr");
		}

		private static void ParseNumber_ParseFloat_Ok(string prefix, string expected, string suffix) {
			var actual = JSNumberValue.ParseNumber(prefix + expected + suffix);
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Type == JSValueType.Float && Math.Abs(actual.CastToFloat() - double.Parse(expected, CultureInfo.InvariantCulture)) < 1e-9);
		}

		private static void ParseNumber_ParseFloat_Fail(string expected) {
			var actual = JSNumberValue.ParseNumber(expected);
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Type == JSValueType.Float && double.IsNaN(actual.CastToFloat()));
		}

		[TestMethod]
		public void ParseNumber_Float() {
			ParseNumber_ParseFloat_Ok(string.Empty, ".0198", string.Empty);
			ParseNumber_ParseFloat_Ok(string.Empty, "1.0198", string.Empty);
			ParseNumber_ParseFloat_Ok(string.Empty, "1.0198e10", string.Empty);
			ParseNumber_ParseFloat_Ok(string.Empty, "1.0198e-10", string.Empty);
			ParseNumber_ParseFloat_Ok(string.Empty, "1.0198e+10", string.Empty);
			ParseNumber_ParseFloat_Fail("1.0198e+1a");
			ParseNumber_ParseFloat_Fail("1.0198e+100000000");
			ParseNumber_ParseFloat_Ok(" ", "1.0198e+10", " ");
		}
	}
}
