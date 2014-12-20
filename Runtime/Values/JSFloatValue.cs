using System;
using System.Globalization;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Вещественное значение
	/// </summary>
	[Serializable]
	public sealed class JSFloatValue : JSNumberValue {
		internal JSFloatValue(double value)
			: base(JSValueType.Float) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		public double Value { get; private set; }
	}
}
