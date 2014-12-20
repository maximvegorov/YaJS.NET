using System.Collections.Generic;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSBoolean
	/// </summary>
	internal sealed class JSBooleanConstructor : JSNativeFunction {
		public JSBooleanConstructor(JSObject inherited)
			: base(inherited) {
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.Boolean);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			var value = args.Count > 0 ? args[0].CastToBoolean() : false;
			if (context == null)
				return (JSValue.Create(value));
			else
				return (vm.NewBoolean(value));
		}
	}
}
