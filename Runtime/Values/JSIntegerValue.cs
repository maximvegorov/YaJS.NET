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
	}
}
