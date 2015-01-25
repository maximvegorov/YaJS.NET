using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
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

		public override JSValue Construct(ExecutionThread thread, LocalScope outerScope, List<JSValue> args) {
			return (VM.NewNumber(args.Count > 0 ? args[0].ToNumber() : 0));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (args.Count > 0 ? args[0].ToNumber() : 0);
		}

		public override int ParameterCount {
			get { return (1); }
		}
	}
}
