using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
	/// <summary>
	/// Native-конструктор JSObject
	/// </summary>
	internal sealed class JSObjectConstructor : JSNativeFunction {
		public JSObjectConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.Object);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (args.Length == 0 ? VM.NewObject() : args[0].ToObject(VM));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (1); } }
	}
}
