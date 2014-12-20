using System;
using System.Globalization;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Целочисленное значение
	/// </summary>
	[Serializable]
	public sealed class JSIntegerValue : JSNumberValue {
		internal JSIntegerValue(int value)
			: base(JSValueType.Integer) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		public int Value { get; private set; }
	}
}
