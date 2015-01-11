using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;
	using Runtime.Values;

	/// <summary>
	/// Native-конструктор JSNumber
	/// </summary>
	internal sealed class JSNumberConstructor : JSNativeFunction {
		public JSNumberConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype() {
			return (VM.Number);
		}

		public override JSValue Construct(LocalScope outerScope, List<JSValue> args) {
			return (VM.NewNumber(args.Count > 0 ? args[0].ToNumber() : (JSNumberValue)0));
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (args.Count > 0 ? args[0].ToNumber() : 0);
		}
	}
}
