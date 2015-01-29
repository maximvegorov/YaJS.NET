using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Tests.Values {
	[TestClass]
	public sealed class JSUndefinedValueTests {
		[TestMethod]
		public void StrictEqualsTo() {
			Assert.IsTrue(JSValue.Undefined.StrictEqualsTo(JSValue.Undefined));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(JSValue.Null));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(false));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo((JSNumberValue)0));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo((JSNumberValue)0.0));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(""));
		}
	}
}
