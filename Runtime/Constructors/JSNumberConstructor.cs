using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSNumber
	/// </summary>
	internal sealed class JSNumberConstructor : JSNativeFunction {
		public JSNumberConstructor(JSObject inherited)
			: base(inherited) {
		}

		public static void InitPrototype(JSObject proto, JSObject functionPrototype) {
			Contract.Requires(proto != null);
			Contract.Requires(functionPrototype != null);
			// TODO
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.Number);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			var value = args.Count > 0 ? args[0].CastToFloat() : 0.0;
			if (context == null)
				return (JSValue.Create(value));
			else
				return (vm.NewNumber(value));
		}
	}
}
