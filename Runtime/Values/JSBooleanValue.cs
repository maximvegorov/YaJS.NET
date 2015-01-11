using System;

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

		public override string TypeOf() {
			return ("boolean");
		}
	}
}
