using System.Collections.Generic;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
	/// <summary>
	/// Native-конструктор JSArray
	/// </summary>
	internal sealed class JSArrayConstructor : JSNativeFunction {
		public JSArrayConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.Array);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (VM.NewArray(new List<JSValue>(args)));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (0); } }
	}
}
