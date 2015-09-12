using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Tests.Values {
	[TestClass]
	public sealed class JSNullValueTests {
		[TestMethod]
		public void StrictEqualsTo() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			Assert.IsTrue(JSValue.Null.StrictEqualsTo(JSValue.Null));
			Assert.IsFalse(JSValue.Null.StrictEqualsTo(JSValue.Undefined));
			Assert.IsFalse(JSValue.Null.StrictEqualsTo(false));
			Assert.IsFalse(JSValue.Null.StrictEqualsTo((JSNumberValue)0));
			Assert.IsFalse(JSValue.Null.StrictEqualsTo((JSNumberValue)0.0));
			Assert.IsFalse(JSValue.Null.StrictEqualsTo(""));
			Assert.IsFalse(JSValue.Null.StrictEqualsTo(vm.NewObject()));
		}

		[TestMethod]
		public void ConvEqualsTo() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			Assert.IsTrue(JSValue.Null.ConvEqualsTo(JSValue.Null));
			Assert.IsTrue(JSValue.Null.ConvEqualsTo(JSValue.Undefined));
			Assert.IsFalse(JSValue.Null.ConvEqualsTo(false));
			Assert.IsFalse(JSValue.Null.ConvEqualsTo((JSNumberValue)0));
			Assert.IsFalse(JSValue.Null.ConvEqualsTo((JSNumberValue)0.0));
			Assert.IsFalse(JSValue.Null.ConvEqualsTo(""));
			Assert.IsFalse(JSValue.Null.ConvEqualsTo(vm.NewObject()));
		}

		[TestMethod]
		public void TypeOf() {
			Assert.IsTrue(JSValue.Null.TypeOf() == "null");
		}

		[TestMethod]
		public void Cast() {
			Assert.IsFalse(JSValue.Null.CastToBoolean());
			Assert.AreEqual(JSValue.Null.CastToInteger(), 0);
			Assert.AreEqual(JSValue.Null.CastToFloat(), 0.0);
			Assert.AreEqual(JSValue.Null.CastToString(), "null");
		}

		[TestMethod]
		public void ToNumber() {
			Assert.AreEqual(JSValue.Null.ToNumber().CastToInteger(), 0);
		}
	}
}
