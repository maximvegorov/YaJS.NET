using System;
using System.Collections.Generic;

namespace YaJS.Runtime.Objects.Prototypes {
	public sealed class JSArrayPrototype : JSUnenumerableLazyInitPrototype {
		private static readonly Dictionary<string, Func<VirtualMachine, JSValue>> LazyMembers =
			new Dictionary<string, Func<VirtualMachine, JSValue>>();

		static JSArrayPrototype() {
		}

		public JSArrayPrototype(VirtualMachine vm, JSObject inherited = null) : base(vm, LazyMembers, inherited) {
		}
	}
}
