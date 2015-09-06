using System;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект-wrapper для логических значений
	/// </summary>
	internal sealed class JSBoolean : JSObject {
		private readonly bool _value;

		public JSBoolean(VirtualMachine vm, bool value, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(inherited == vm.Boolean);
			_value = value;
		}

		public override string ToString() {
			return (_value.ToString());
		}

		public override void CastToPrimitiveValue(ExecutionThread thread, Action<JSValue> onCompleteCallback) {
			onCompleteCallback(_value);
		}
	}
}
