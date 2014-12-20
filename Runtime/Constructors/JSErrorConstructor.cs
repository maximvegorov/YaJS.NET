using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSError
	/// </summary>
	internal sealed class JSErrorConstructor : JSNativeFunction {
		public JSErrorConstructor(JSObject inherited)
			: base(inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.Error);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			return (vm.NewError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}
