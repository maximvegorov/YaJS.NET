using System;
using System.Globalization;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Вещественное значение
	/// </summary>
	[Serializable]
	internal sealed class JSFloatValue : JSNumberValue {
		private readonly double _value;

		public JSFloatValue(double value)
			: base(JSValueType.Float) {
			_value = value;
		}

		public override bool Equals(object other) {
			var otherValue = other as JSFloatValue;
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
			return (_value + value.CastToFloat());
		}

		public override JSNumberValue Minus(JSNumberValue value) {
			return (_value - value.CastToFloat());
		}

		public override JSNumberValue Mul(JSNumberValue value) {
			return (_value * value.CastToFloat());
		}

		public override JSNumberValue Mod(JSNumberValue value) {
			return (_value % value.CastToFloat());
		}

		public override bool Lt(JSNumberValue value) {
			return (_value < value.CastToFloat());
		}

		public override bool Lte(JSNumberValue value) {
			return (_value <= value.CastToFloat());
		}

		public override bool ConvEqualsTo(JSValue value) {
			if (value.Type == JSValueType.String)
				value = value.ToNumber();
			return (_value == value.CastToFloat());
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Float && _value == value.CastToFloat());
		}

		public override bool CastToBoolean() {
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			return (_value != 0.0 && !double.IsNaN(_value));
		}

		public override int CastToInteger() {
			return ((int)_value);
		}

		public override double CastToFloat() {
			return (_value);
		}

		public override string CastToString() {
			return (_value.ToString(CultureInfo.InvariantCulture));
		}
	}
}
