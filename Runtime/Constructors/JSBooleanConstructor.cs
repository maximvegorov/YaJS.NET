using System.Collections.Generic;
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

		public override JSValue Construct(ExecutionThread thread, LocalScope outerScope, List<JSValue> args) {
			return (VM.NewBoolean(args.Count > 0 && args[0].CastToBoolean()));
		}

		public override JSValue Invoke(
			ExecutionThread thread,
			JSObject context,
			LocalScope outerScope,
			List<JSValue> args
			) {
			return (args.Count > 0 && args[0].CastToBoolean());
		}

		public override int ParameterCount { get { return (1); } }
	}
}
