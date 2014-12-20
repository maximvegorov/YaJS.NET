using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Логическое значение
	/// </summary>
	[Serializable]
	public sealed class JSBooleanValue : JSValue {
		internal JSBooleanValue(bool value)
			: base(JSValueType.Boolean) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString());
		}

		public bool Value { get; private set; }
	}
}
