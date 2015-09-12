using System;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Логическое значение
	/// </summary>
	[Serializable]
	internal sealed class JSBooleanValue : JSValue {
		private readonly bool _value;

		public JSBooleanValue(bool value)
			: base(JSValueType.Boolean) {
			_value = value;
		}

		public override bool Equals(object other) {
			var otherValue = other as JSBooleanValue;
			return (otherValue != null && _value.Equals(otherValue._value));
		}

		public override int GetHashCode() {
			return (_value.GetHashCode());
		}

		public override string ToString() {
			return (_value.ToString());
		}

		public override bool ConvEqualsTo(JSValue value) {
			if (value.Type == JSValueType.String)
				value = value.ToNumber();
			switch (value.Type) {
				case JSValueType.Boolean:
					return (_value && value.CastToBoolean());
				case JSValueType.Integer:
					return (CastToInteger() == value.CastToInteger());
				case JSValueType.Float:
					return (CastToFloat() == value.CastToFloat());
				default:
					return (false);
			}
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Boolean && _value == value.CastToBoolean());
		}

		public override string TypeOf() {
			return ("boolean");
		}

		public override bool CastToBoolean() {
			return (_value);
		}

		public override int CastToInteger() {
			return (_value ? 1 : 0);
		}

		public override double CastToFloat() {
			return (_value ? 1 : 0);
		}

		public override string CastToString() {
			return (_value ? "true" : "false");
		}

		public override JSNumberValue ToNumber() {
			return (_value ? 1 : 0);
		}

		public override JSObject ToObject(VirtualMachine vm) {
			return (vm.NewBoolean(_value));
		}
	}
}
