using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Undefined
	/// </summary>
	[Serializable]
	public sealed class JSUndefinedValue : JSValue {
		internal JSUndefinedValue() : base(JSValueType.Undefined) { }

		public override string ToString() {
			return ("undefined");
		}
	}
}
