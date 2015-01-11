using System.Collections.Generic;

namespace YaJS.Runtime.Constructors.Errors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSSyntaxError
	/// </summary>
	internal sealed class JSSyntaxErrorConstructor : JSNativeFunction {
		public JSSyntaxErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.SyntaxError);
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (VM.NewSyntaxError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}
