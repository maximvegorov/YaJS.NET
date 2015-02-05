using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Tests.Objects {
	[TestClass]
	public sealed class JSLazyInitObjectTests {
		private class TestPrototype : JSLazyInitObject {
			public TestPrototype(VirtualMachine vm, JSObject inherited)
				: base(vm, GetLazyMembers(), inherited) {
			}

			private static Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> GetLazyMembers() {
				return (new Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> {
					{ "a", (vm, o) => "a" }
				});
			}
		}

		[TestMethod]
		public void GetEnumerator() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto = new TestPrototype(vm, null);
			proto.OwnMembers.Add("b", "b");
			var expected = new HashSet<string> { "a", "b" };
			var actual = new HashSet<string>();
			var enumerator = proto.GetEnumerator();
			while (enumerator.MoveNext())
				actual.Add(enumerator.Current.CastToString());
			Assert.IsTrue(actual.SetEquals(expected));
		}

		[TestMethod]
		public void ContainsMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto = new TestPrototype(vm, null);
			proto.OwnMembers.Add("b", "b");
			Assert.IsTrue(proto.ContainsMember("a"));
			Assert.IsTrue(proto.ContainsMember("b"));
		}

		[TestMethod]
		public void GetMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto = new TestPrototype(vm, null);
			proto.OwnMembers.Add("b", "b");
			var actual = proto.GetMember("a");
			Assert.IsTrue(proto.OwnMembers.ContainsKey("a"));
			Assert.IsTrue(actual.StrictEqualsTo("a"));
			Assert.IsTrue(proto.GetMember("b").StrictEqualsTo("b"));
		}

		[TestMethod]
		public void DeleteMember() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto = new TestPrototype(vm, null);
			proto.OwnMembers.Add("b", "b");
			Assert.IsFalse(proto.DeleteMember("a"));
			Assert.IsTrue(proto.DeleteMember("b"));
			Assert.IsFalse(proto.OwnMembers.ContainsKey("b"));
		}
	}
}
