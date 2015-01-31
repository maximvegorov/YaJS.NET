using System;
using System.Collections.Generic;

namespace YaJS.Runtime.Objects.Prototypes {
	public sealed class JSBooleanPrototype : JSUnenumerableLazyInitPrototype {
		private static readonly Dictionary<string, Func<VirtualMachine, JSValue>> LazyMembers =
			new Dictionary<string, Func<VirtualMachine, JSValue>>();

		static JSBooleanPrototype() {
		}

		public JSBooleanPrototype(VirtualMachine vm, JSObject inherited = null)
			: base(vm, LazyMembers, inherited) {
		}
	}
}
