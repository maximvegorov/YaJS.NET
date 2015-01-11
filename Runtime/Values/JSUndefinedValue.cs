using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Undefined
	/// </summary>
	[Serializable]
	internal sealed class JSUndefinedValue : JSValue {
		private const string UndefinedString = "undefined";

		public JSUndefinedValue() : base(JSValueType.Undefined) { }

		public override string ToString() {
			return (UndefinedString);
		}

		public override string TypeOf() {
			return (UndefinedString);
		}
	}
}
