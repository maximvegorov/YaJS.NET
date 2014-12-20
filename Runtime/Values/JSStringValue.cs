using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Строковое значение
	/// </summary>
	[Serializable]
	public sealed class JSStringValue : JSValue {
		internal JSStringValue(string value)
			: base(JSValueType.String) {
			Value = value ?? string.Empty;
		}

		public override string ToString() {
			return (Value);
		}

		public string Value { get; private set; }
	}
}
