using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Null
	/// </summary>
	[Serializable]
	internal sealed class JSNullValue : JSValue {
		private const string NullString = "null";

		public JSNullValue()
			: base(JSValueType.Null) {
		}

		public override string ToString() {
			return (NullString);
		}

		public override bool ConvEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Undefined || value.Type == JSValueType.Null);
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Undefined);
		}

		public override string TypeOf() {
			return (NullString);
		}

		public override bool CastToBoolean() {
			return (false);
		}

		public override int CastToInteger() {
			return (0);
		}

		public override double CastToFloat() {
			return (0);
		}

		public override string CastToString() {
			return (NullString);
		}

		public override JSNumberValue ToNumber() {
			return (0);
		}
	}
}
