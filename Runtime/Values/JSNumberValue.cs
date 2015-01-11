using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Базовый класс для числовых значений
	/// </summary>
	[Serializable]
	public abstract class JSNumberValue : JSValue {
		protected JSNumberValue(JSValueType type)
			: base(type) {
		}

		public override string TypeOf() {
			return ("number");
		}

		public static implicit operator JSNumberValue(int value) {
			return (new JSIntegerValue(value));
		}

		public static implicit operator JSNumberValue(double value) {
			return (new JSFloatValue(value));
		}
	}
}
