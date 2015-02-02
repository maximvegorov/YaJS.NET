using System;
using System.Collections.Generic;
using System.Linq;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект с отложенной инициализацией с неперечисляемыми свойствами
	/// </summary>
	public abstract class JSUnenumerableLazyInitObject : JSLazyInitObject {
		protected JSUnenumerableLazyInitObject(
			VirtualMachine vm,
			Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> lazyMemberFactories,
			JSObject inherited)
			: base(vm, lazyMemberFactories, inherited) {
		}

		protected override IEnumerable<string> GetEnumerableMembers() {
			return (Enumerable.Empty<string>());
		}
	}
}
