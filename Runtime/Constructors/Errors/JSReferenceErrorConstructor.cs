using System.Collections.Generic;

namespace YaJS.Runtime.Constructors.Errors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSReferenceError
	/// </summary>
	internal sealed class JSReferenceErrorConstructor : JSNativeFunction {
		public JSReferenceErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.ReferenceError);
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (VM.NewReferenceError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}
