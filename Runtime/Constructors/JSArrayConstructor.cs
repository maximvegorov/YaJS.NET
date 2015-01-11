using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSArray
	/// </summary>
	internal sealed class JSArrayConstructor : JSNativeFunction {
		public JSArrayConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype() {
			return (VM.Array);
		}

		public override JSValue Construct(LocalScope outerScope, List<JSValue> args) {
			return (VM.NewArray(args));
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (Construct(outerScope, args));
		}
	}
}
