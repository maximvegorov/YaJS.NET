using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Tests.Objects {
	[TestClass]
	public sealed class JSObjectTests {
		[TestMethod]
		public void ContainsMember_OwnProperty() {
			const string property1 = "a";
			const string property2 = "b";
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto1 = new JSObject(vm, null);
			var proto2 = new JSObject(vm, proto1);
			var obj = new JSObject(vm, proto2);
			obj.OwnMembers.Add(property1, true);
			Assert.IsTrue(obj.ContainsMember(property1));
			Assert.IsFalse(obj.ContainsMember(property2));
		}

		[TestMethod]
		public void ContainsMember_Prototype() {
			const string property1 = "a";
			const string property2 = "b";
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto1 = new JSObject(vm, null);
			proto1.OwnMembers.Add(property1, true);
			var proto2 = new JSObject(vm, proto1);
			var obj = new JSObject(vm, proto2);
			Assert.IsTrue(obj.ContainsMember(property1));
			Assert.IsFalse(obj.ContainsMember(property2));
		}

		[TestMethod]
		public void GetMember_OwnProperty() {
			const string property1 = "a";
			const string property2 = "b";
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto1 = new JSObject(vm, null);
			var proto2 = new JSObject(vm, proto1);
			var obj = new JSObject(vm, proto2);
			obj.OwnMembers.Add(property1, true);
			Assert.IsTrue(obj.GetMember(property1).StrictEqualsTo(true));
			Assert.IsTrue(obj.GetMember(property2).Type == JSValueType.Undefined);
		}

		[TestMethod]
		public void GetMember_Prototype() {
			const string property1 = "a";
			const string property2 = "b";
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto1 = new JSObject(vm, null);
			proto1.OwnMembers.Add(property1, true);
			var proto2 = new JSObject(vm, proto1);
			var obj = new JSObject(vm, proto2);
			Assert.IsTrue(obj.GetMember(property1).StrictEqualsTo(true));
			Assert.IsTrue(obj.GetMember(property2).Type == JSValueType.Undefined);
		}

		[TestMethod]
		public void SetMember_Simple() {
			const string property = "a";
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var obj = vm.NewObject();
			obj.SetMember(property, true);
			Assert.IsTrue(obj.OwnMembers.ContainsKey(property));
			Assert.IsTrue(obj.GetMember(property).StrictEqualsTo(true));
		}

		[TestMethod]
		public void DeleteMember_Simple() {
			const string property = "a";
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var obj = vm.NewObject();
			obj.SetMember(property, true);
			obj.DeleteMember(property);
			Assert.IsFalse(obj.ContainsMember(property));
		}
	}
}
