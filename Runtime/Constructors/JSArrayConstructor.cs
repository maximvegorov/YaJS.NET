using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
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

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, List<JSValue> args) {
			return (VM.NewArray(args));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, List<JSValue> args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (0); } }
	}
}
