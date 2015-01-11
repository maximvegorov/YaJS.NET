using System.Collections.Generic;

namespace YaJS.Runtime.Constructors.Errors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSTypeError
	/// </summary>
	internal sealed class JSTypeErrorConstructor : JSNativeFunction {
		public JSTypeErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.TypeError);
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (VM.NewTypeError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}
