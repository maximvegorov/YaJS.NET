using System.Collections.Generic;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors.Errors {
	/// <summary>
	/// Native-конструктор JSRangeError
	/// </summary>
	internal sealed class JSRangeErrorConstructor : JSNativeFunction {
		public JSRangeErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.RangeError);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, List<JSValue> args) {
			return (VM.NewRangeError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, List<JSValue> args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (1); } }
	}
}
