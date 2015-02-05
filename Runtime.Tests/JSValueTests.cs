using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Tests {
	[TestClass]
	public sealed class JSValueTests {
		#region InstanceOf

		[TestMethod]
		[ExpectedException(typeof (TypeErrorException))]
		public void InstanceOf_Undefined() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();
			JSValue.Undefined.IsInstanceOf(constructor);
		}

		[TestMethod]
		[ExpectedException(typeof (TypeErrorException))]
		public void InstanceOf_Null() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();
			JSValue.Null.IsInstanceOf(constructor);
		}

		[TestMethod]
		[ExpectedException(typeof (TypeErrorException))]
		public void InstanceOf_Boolean() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();
			((JSValue)false).IsInstanceOf(constructor);
		}

		[TestMethod]
		[ExpectedException(typeof (TypeErrorException))]
		public void InstanceOf_Integer() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();
			((JSNumberValue)0).IsInstanceOf(constructor);
		}

		[TestMethod]
		[ExpectedException(typeof (TypeErrorException))]
		public void InstanceOf_Float() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();
			((JSNumberValue)1.0).IsInstanceOf(constructor);
		}

		[TestMethod]
		[ExpectedException(typeof (TypeErrorException))]
		public void InstanceOf_String() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();
			((JSValue)"abc").IsInstanceOf(constructor);
		}

		[TestMethod]
		public void InstanceOf_BuildinCoreObjects() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var constructor = vm.GlobalObject.OwnMembers["Object"].RequireFunction();

			Assert.IsTrue(vm.NewObject().IsInstanceOf(constructor));

			Assert.IsTrue(vm.NewBoolean(false).IsInstanceOf(constructor));
			Assert.IsTrue(vm.NewBoolean(false).IsInstanceOf(vm.GlobalObject.OwnMembers["Boolean"].RequireFunction()));

			Assert.IsTrue(vm.NewNumber(0).IsInstanceOf(constructor));
			Assert.IsTrue(vm.NewNumber(0).IsInstanceOf(vm.GlobalObject.OwnMembers["Number"].RequireFunction()));

			Assert.IsTrue(vm.NewNumber(0.0).IsInstanceOf(constructor));
			Assert.IsTrue(vm.NewNumber(0.0).IsInstanceOf(vm.GlobalObject.OwnMembers["Number"].RequireFunction()));

			Assert.IsTrue(vm.NewString("abc").IsInstanceOf(constructor));
			Assert.IsTrue(vm.NewString("abc").IsInstanceOf(vm.GlobalObject.OwnMembers["String"].RequireFunction()));

			Assert.IsTrue(vm.NewArray(new List<JSValue>()).IsInstanceOf(constructor));
			Assert.IsTrue(vm.NewArray(new List<JSValue>()).IsInstanceOf(vm.GlobalObject.OwnMembers["Array"].RequireFunction()));

			Assert.IsTrue(vm.NewError("abc").IsInstanceOf(constructor));
			Assert.IsTrue(vm.NewError("abc").IsInstanceOf(vm.GlobalObject.OwnMembers["Error"].RequireFunction()));

			Assert.IsFalse(vm.NewNumber(0).IsInstanceOf(vm.GlobalObject.OwnMembers["String"].RequireFunction()));
		}

		#endregion
	}
}
