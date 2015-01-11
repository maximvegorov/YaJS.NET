using System.Collections.Generic;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

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

		public override JSValue Construct(LocalScope outerScope, List<JSValue> args) {
			return (VM.NewBoolean(args.Count > 0 && args[0].CastToBoolean()));
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (args.Count > 0 && args[0].CastToBoolean());
		}
	}
}
