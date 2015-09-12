using System;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект-wrapper для числовых значений
	/// </summary>
	internal sealed class JSNumber : JSObject {
		private readonly JSNumberValue _value;

		public JSNumber(VirtualMachine vm, JSNumberValue value, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(value != null);
			Contract.Requires(inherited == vm.Number);
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
