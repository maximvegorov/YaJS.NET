using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors.Errors {
	/// <summary>
	/// Native-конструктор JSSyntaxError
	/// </summary>
	internal sealed class JSSyntaxErrorConstructor : JSNativeFunction {
		public JSSyntaxErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.SyntaxError);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (VM.NewSyntaxError(args.Length > 0 ? args[0].CastToString() : string.Empty));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (1); } }
	}
}
