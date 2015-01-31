using System;
using System.Collections.Generic;
using System.Linq;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Прототип объекта с отложенной инициализацией с неперечисляемыми свойствами
	/// </summary>
	public abstract class JSUnenumerableLazyInitPrototype : JSLazyInitPrototype {
		protected JSUnenumerableLazyInitPrototype(
			VirtualMachine vm,
			Dictionary<string, Func<VirtualMachine, JSValue>> lazyMemberFactories,
			JSObject inherited)
			: base(vm, lazyMemberFactories, inherited) {
		}

		protected override IEnumerable<string> GetEnumerableMembers() {
			return (Enumerable.Empty<string>());
		}
	}
}
