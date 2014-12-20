using System.Collections.Generic;

namespace YaJS.Runtime.Constructors.Errors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSSyntaxError
	/// </summary>
	internal sealed class JSSyntaxErrorConstructor : JSNativeFunction {
		public JSSyntaxErrorConstructor(JSObject inherited)
			: base(inherited) {
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.SyntaxError);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			return (vm.NewSyntaxError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}
	}
}

