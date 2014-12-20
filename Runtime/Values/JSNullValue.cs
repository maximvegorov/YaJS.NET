using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Null
	/// </summary>
	[Serializable]
	public sealed class JSNullValue : JSValue {
		internal JSNullValue() : base(JSValueType.Null) { }

		public override string ToString() {
			return ("null");
		}
	}
}
