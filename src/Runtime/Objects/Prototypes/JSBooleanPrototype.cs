using System;
using System.Collections.Generic;

namespace YaJS.Runtime.Objects.Prototypes {
	public sealed class JSBooleanPrototype : JSUnenumerableLazyInitObject {
		private static readonly Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> LazyMembers =
			new Dictionary<string, Func<VirtualMachine, JSObject, JSValue>>();

		static JSBooleanPrototype() {
		}

		public JSBooleanPrototype(VirtualMachine vm, JSObject inherited = null)
			: base(vm, LazyMembers, inherited) {
		}
	}
}
