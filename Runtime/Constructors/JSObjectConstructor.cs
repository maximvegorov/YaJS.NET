using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
	/// <summary>
	/// Native-конструктор JSObject
	/// </summary>
	internal sealed class JSObjectConstructor : JSNativeFunction {
		public JSObjectConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype() {
			return (VM.Object);
		}

		public override JSValue Construct(ExecutionThread thread, LocalScope outerScope, List<JSValue> args) {
			return (args.Count == 0 ? VM.NewObject() : args[0].ToObject(VM));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount {
			get { return (1); }
		}
	}
}
