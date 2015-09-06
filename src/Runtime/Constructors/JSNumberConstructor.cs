using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
	/// <summary>
	/// Native-конструктор JSNumber
	/// </summary>
	internal sealed class JSNumberConstructor : JSNativeFunction {
		public JSNumberConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.Number);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (VM.NewNumber(args.Length > 0 ? args[0].ToNumber() : 0));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (args.Length > 0 ? args[0].ToNumber() : 0);
		}

		public override int ParameterCount { get { return (1); } }
	}
}
