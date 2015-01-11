using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	using Runtime.Values;

	/// <summary>
	/// Объект-wrapper для числовых значений
	/// </summary>
	internal sealed class JSNumber : JSObject {
		private readonly JSNumberValue _value;

		public JSNumber(VirtualMachine vm, JSNumberValue value, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(value != null);
			Contract.Requires(inherited != null);
			_value = value;
		}

		public override string ToString() {
			return (_value.ToString());
		}
	}
}
