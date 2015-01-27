using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YaJS.Runtime.Exceptions;

namespace YaJS.Runtime.Tests {
	[TestClass]
	public sealed class JSValueTests {
		[TestMethod]
		[ExpectedException(typeof(TypeErrorException))]
		public void InstanceOf_Undefined() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);

		}

		[TestMethod]
		[ExpectedException(typeof(TypeErrorException))]
		public void InstanceOf_Simple() {
			var compiler = new Mock<ICompilerServices>();
			var vm = new VirtualMachine(compiler.Object);

		}
	}
}
