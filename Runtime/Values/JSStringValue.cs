using System;

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

		public override string TypeOf() {
			return ("string");
		}
	}
}
