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
	}
}
