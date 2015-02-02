using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
	/// <summary>
	/// Native-конструктор JSBoolean
	/// </summary>
	internal sealed class JSBooleanConstructor : JSNativeFunction {
		public JSBooleanConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.Boolean);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (VM.NewBoolean(args.Length > 0 && args[0].CastToBoolean()));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (args.Length > 0 && args[0].CastToBoolean());
		}

		public override int ParameterCount { get { return (1); } }
	}
}
