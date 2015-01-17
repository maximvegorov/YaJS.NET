using System;
using System.Globalization;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Целочисленное значение
	/// </summary>
	[Serializable]
	internal sealed class JSIntegerValue : JSNumberValue {
		private readonly int _value;

		public JSIntegerValue(int value)
			: base(JSValueType.Integer) {
			_value = value;
		}

		public override bool Equals(object other) {
			var otherValue = other as JSIntegerValue;
			return (otherValue != null && _value.Equals(otherValue._value));
		}

		public override int GetHashCode() {
			return (_value.GetHashCode());
		}

		public override string ToString() {
			return (_value.ToString(CultureInfo.InvariantCulture));
		}

		public override JSNumberValue Neg() {
			return (-_value);
		}

		public override JSNumberValue Inc() {
			return (_value + 1);
		}

		public override JSNumberValue Dec() {
			return (_value - 1);
		}

		public override JSNumberValue Plus(JSNumberValue value) {
			if (value.Type != JSValueType.Integer)
				return (_value + value.CastToFloat());
			return (unchecked(_value + value.CastToInteger()));
		}

		public override JSNumberValue Minus(JSNumberValue value) {
			if (value.Type != JSValueType.Integer)
				return (_value - value.CastToFloat());
			return (unchecked(_value - value.CastToInteger()));
		}

		public override JSNumberValue Mul(JSNumberValue value) {
			if (value.Type != JSValueType.Integer)
				return (_value * value.CastToFloat());
			return (unchecked(_value * value.CastToInteger()));
		}

		public override JSNumberValue Mod(JSNumberValue value) {
			if (value.Type != JSValueType.Integer)
				return (_value % value.CastToFloat());
			return (_value % value.CastToInteger());
		}

		public override bool Lt(JSNumberValue value) {
			if (value.Type != JSValueType.Integer)
				return (_value < value.CastToFloat());
			return (_value < value.CastToInteger());
		}

		public override bool Lte(JSNumberValue value) {
			if (value.Type != JSValueType.Integer)
				return (_value <= value.CastToFloat());
			return (_value <= value.CastToInteger());
		}

		public override bool ConvEqualsTo(JSValue value) {
			if (value.Type == JSValueType.String)
				value = value.ToNumber();
			switch (value.Type) {
				case JSValueType.Integer:
					return (_value == value.CastToInteger());
				case JSValueType.Float:
					return (_value == value.CastToFloat());
				default:
					return (false);
			}
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Integer && _value == value.CastToInteger());
		}

		public override bool CastToBoolean() {
			return (_value != 0);
		}

		public override int CastToInteger() {
			return (_value);
		}

		public override double CastToFloat() {
			return (_value);
		}

		public override string CastToString() {
			return (_value.ToString(CultureInfo.InvariantCulture));
		}

		public override int RequireInteger() {
			return (_value);
		}
	}
}
