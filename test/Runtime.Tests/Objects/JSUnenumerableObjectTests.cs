using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Tests.Objects {
	[TestClass]
	public sealed class JSUnenumerableObjectTests {
		[TestMethod]
		public void GetEnumerator() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);
			var proto = new JSUnenumerableObject(vm, null);
			proto.OwnMembers.Add("a", true);
			var enumerator = proto.GetEnumerator();
			Assert.IsFalse(enumerator.MoveNext());
		}
	}
}
