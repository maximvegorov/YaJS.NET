using System;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Строковое значение
	/// </summary>
	[Serializable]
	internal sealed class JSStringValue : JSValue {
		private readonly string _value;

		public JSStringValue(string value)
			: base(JSValueType.String) {
			_value = value ?? string.Empty;
		}

		public override bool Equals(object other) {
			var otherValue = other as JSStringValue;
			return (otherValue != null && _value.Equals(otherValue._value));
		}

		public override int GetHashCode() {
			return (_value.GetHashCode());
		}

		public override string ToString() {
			return (_value);
		}

		public override bool ConvEqualsTo(JSValue value) {
			if (value.Type == JSValueType.String)
				return (_value == value.CastToString());
			return (value.ToNumber().ConvEqualsTo(value));
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (value.Type == JSValueType.String && _value == value.CastToString());
		}

		public override string TypeOf() {
			return ("string");
		}

		public override bool CastToBoolean() {
			return (!string.IsNullOrEmpty(_value));
		}

		public override string CastToString() {
			return (_value);
		}

		public override JSNumberValue ToNumber() {
			return (JSNumberValue.ParseNumber(_value));
		}

		public override JSObject ToObject(VirtualMachine vm) {
			return (vm.NewString(_value));
		}
	}
}
