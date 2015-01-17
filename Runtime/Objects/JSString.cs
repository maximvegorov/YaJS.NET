using System;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект-wrapper для строковых значений
	/// </summary>
	internal sealed class JSString : JSObject {
		private readonly string _value;

		public JSString(VirtualMachine vm, string value, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(inherited == vm.String);
			_value = value ?? string.Empty;
		}

		public override string ToString() {
			return (_value);
		}

		public override void CastToPrimitiveValue(ExecutionThread thread, Action<JSValue> onCompleteCallback) {
			onCompleteCallback(_value);
		}
	}
}
