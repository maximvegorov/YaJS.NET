using System.Collections.Generic;

namespace YaJS.Runtime.Constructors.Errors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSTypeError
	/// </summary>
	internal sealed class JSTypeErrorConstructor : JSNativeFunction {
		public JSTypeErrorConstructor(JSObject inherited)
			: base(inherited) {
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.TypeError);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			return (vm.NewTypeError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}
