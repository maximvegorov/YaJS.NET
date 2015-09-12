using System;
using System.Collections.Generic;

namespace YaJS.Runtime.Objects.Prototypes {
	public sealed class JSStringPrototype : JSUnenumerableLazyInitObject {
		private static readonly Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> LazyMembers =
			new Dictionary<string, Func<VirtualMachine, JSObject, JSValue>>();

		static JSStringPrototype() {
		}

		public JSStringPrototype(VirtualMachine vm, JSObject inherited = null)
			: base(vm, LazyMembers, inherited) {
		}
	}
}
