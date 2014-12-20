using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSObject
	/// </summary>
	internal sealed class JSObjectConstructor : JSNativeFunction {
		public JSObjectConstructor(JSObject inherited)
			: base(inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.Object);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			if (args.Count == 0)
				return (vm.NewObject());
			else
				return (args[0].ToObject(vm));
		}
	}
}
