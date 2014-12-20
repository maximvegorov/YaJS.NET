using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSArray
	/// </summary>
	internal sealed class JSArrayConstructor : JSNativeFunction {
		public JSArrayConstructor(JSObject inherited)
			: base(inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.Array);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			return (vm.NewArray(args));
		}
	}
}
