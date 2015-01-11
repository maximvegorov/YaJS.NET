using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Null
	/// </summary>
	[Serializable]
	internal sealed class JSNullValue : JSValue {
		private const string NullString = "null";

		public JSNullValue() : base(JSValueType.Null) { }

		public override string ToString() {
			return (NullString);
		}

		public override string TypeOf() {
			return (NullString);
		}
	}
}
