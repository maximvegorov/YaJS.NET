using System;
using System.Collections.Generic;
using YaJS.Runtime;
using YaJS.Runtime.Objects;

namespace yajs.Objects {
	public sealed partial class JSConsole : JSLazyInitObject {
		private static readonly Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> LazyMembers =
			new Dictionary<string, Func<VirtualMachine, JSObject, JSValue>>();

		static JSConsole() {
			LazyMembers.Add("log", (vm, o) => new Log(vm, vm.Function));
			LazyMembers.Add("info", (vm, o) => o.GetMember("log"));
			LazyMembers.Add("error", (vm, o) => new Error(vm, vm.Function));
			LazyMembers.Add("warn", (vm, o) => o.GetMember("error"));
		}

		public JSConsole(VirtualMachine vm)
			: base(vm, LazyMembers, null) {
		}
	}
}
