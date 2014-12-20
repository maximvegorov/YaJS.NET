using System.Collections.Generic;

namespace YaJS.Runtime.Constructors.Errors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSReferenceError
	/// </summary>
	internal sealed class JSReferenceErrorConstructor : JSNativeFunction {
		public JSReferenceErrorConstructor(JSObject inherited)
			: base(inherited) {
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.ReferenceError);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			return (vm.NewReferenceError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}
