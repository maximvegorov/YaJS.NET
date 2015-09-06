using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
	/// <summary>
	/// Native-конструктор JSString
	/// </summary>
	internal sealed class JSStringConstructor : JSNativeFunction {
		public JSStringConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.String);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (VM.NewString(args.Length > 0 ? args[0].CastToString() : string.Empty));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (args.Length > 0 ? args[0].CastToString() : string.Empty);
		}

		public override int ParameterCount { get { return (1); } }
	}
}
