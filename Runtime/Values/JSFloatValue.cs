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
	}
}
