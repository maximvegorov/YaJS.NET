using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Tests.Values {
	[TestClass]
	public sealed class JSUndefinedValueTests {
		[TestMethod]
		public void StrictEqualsTo() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			Assert.IsTrue(JSValue.Undefined.StrictEqualsTo(JSValue.Undefined));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(JSValue.Null));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(false));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo((JSNumberValue)0));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo((JSNumberValue)0.0));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(""));
			Assert.IsFalse(JSValue.Undefined.StrictEqualsTo(vm.NewObject()));
		}

		[TestMethod]
		public void ConvEqualsTo() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			Assert.IsTrue(JSValue.Undefined.ConvEqualsTo(JSValue.Undefined));
			Assert.IsTrue(JSValue.Undefined.ConvEqualsTo(JSValue.Null));
			Assert.IsFalse(JSValue.Undefined.ConvEqualsTo(false));
			Assert.IsFalse(JSValue.Undefined.ConvEqualsTo((JSNumberValue)0));
			Assert.IsFalse(JSValue.Undefined.ConvEqualsTo((JSNumberValue)0.0));
			Assert.IsFalse(JSValue.Undefined.ConvEqualsTo(""));
			Assert.IsFalse(JSValue.Undefined.ConvEqualsTo(vm.NewObject()));
		}

		[TestMethod]
		public void TypeOf() {
			Assert.IsTrue(JSValue.Undefined.TypeOf() == "undefined");
		}

		[TestMethod]
		public void Cast() {
			Assert.IsFalse(JSValue.Undefined.CastToBoolean());
			Assert.AreEqual(JSValue.Undefined.CastToInteger(), 0);
			Assert.IsTrue(double.IsNaN(JSValue.Undefined.CastToFloat()));
			Assert.AreEqual(JSValue.Undefined.CastToString(), "undefined");
		}

		[TestMethod]
		public void ToNumber() {
			Assert.IsTrue(double.IsNaN(JSValue.Undefined.ToNumber().CastToFloat()));
		}
	}
}
