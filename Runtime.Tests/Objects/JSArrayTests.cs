using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Tests.Objects {
	[TestClass]
	public sealed class JSArrayTests {
		private readonly JSValue _property1 = "a";
		private readonly JSNumberValue _property2 = 0;
		private readonly JSValue _property3 = "c";
		private readonly JSValue _property4 = "d";
		private readonly JSNumberValue _property5 = 3;
		private readonly JSNumberValue _expected1 = 1;
		private readonly JSNumberValue _expected2 = 2;
		private readonly JSNumberValue _expected3 = 2;

		[TestMethod]
		public void ContainsMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var arr = vm.NewArray(new List<JSValue>());
			arr.SetMember(_property1, _expected1);
			arr.SetMember(_property2, _expected2);
			arr.SetMember(_property3, _expected3);
			Assert.IsTrue(arr.ContainsMember(_property1));
			Assert.IsTrue(arr.ContainsMember(_property2));
			Assert.IsTrue(arr.ContainsMember(_property3));
			Assert.IsFalse(arr.ContainsMember(_property4));
			Assert.IsFalse(arr.ContainsMember(_property5));
		}

		[TestMethod]
		public void GetMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var arr = vm.NewArray(new List<JSValue>());
			arr.SetMember(_property1, _expected1);
			arr.SetMember(_property2, _expected2);
			arr.SetMember(_property3, _expected3);
			Assert.IsTrue(arr.GetMember(_property1).StrictEqualsTo(_expected1));
			Assert.IsTrue(arr.GetMember(_property2).StrictEqualsTo(_expected2));
			Assert.IsTrue(arr.GetMember(_property3).StrictEqualsTo(_expected3));
			Assert.IsTrue(arr.GetMember(_property4).StrictEqualsTo(JSValue.Undefined));
			Assert.IsTrue(arr.GetMember(_property5).StrictEqualsTo(JSValue.Undefined));
		}

		[TestMethod]
		public void SetMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var arr = vm.NewArray(new List<JSValue>());
			arr.SetMember(_property1, _expected1);
			arr.SetMember(_property2, _expected2);
			arr.SetMember(_property3, _expected3);
			arr.SetMember(_property5, _expected3);
			Assert.IsTrue(arr.ContainsMember(_property5));
			Assert.IsTrue((arr.GetMember("length").StrictEqualsTo(_property5.Inc())));
			Assert.IsTrue(arr.GetMember(_property5).StrictEqualsTo(_expected3));
		}

		[TestMethod]
		public void SetMember_length() {
			var expectedLength = (JSNumberValue)4;
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var arr = vm.NewArray(new List<JSValue>());
			arr.SetMember("length", expectedLength);
			Assert.IsTrue((arr.GetMember("length").StrictEqualsTo(expectedLength)));
		}

		[TestMethod]
		public void DeleteMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var arr = vm.NewArray(new List<JSValue>());
			arr.SetMember(_property1, _expected1);
			arr.SetMember(_property2, _expected2);
			arr.SetMember(_property3, _expected3);
			arr.DeleteMember(_property2);
			Assert.IsFalse(arr.ContainsMember(_property2));
			Assert.IsTrue(arr.GetMember(_property2).StrictEqualsTo(JSValue.Undefined));
			Assert.IsTrue(arr.GetMember("length").StrictEqualsTo((JSNumberValue)1));
		}
	}
}
