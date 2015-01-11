using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSString
	/// </summary>
	internal sealed class JSStringConstructor : JSNativeFunction {
		public JSStringConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype() {
			return (VM.String);
		}

		public override JSValue Construct(LocalScope outerScope, List<JSValue> args) {
			return (VM.NewString(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (args.Count > 0 ? args[0].CastToString() : string.Empty);
		}
	}
}
