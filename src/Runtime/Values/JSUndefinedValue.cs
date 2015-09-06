using System;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Undefined
	/// </summary>
	[Serializable]
	internal sealed class JSUndefinedValue : JSValue {
		private const string UndefinedString = "undefined";

		public JSUndefinedValue()
			: base(JSValueType.Undefined) {
		}

		public override string ToString() {
			return (UndefinedString);
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Undefined);
		}

		public override bool ConvEqualsTo(JSValue value) {
			return (value.Type == JSValueType.Undefined || value.Type == JSValueType.Null);
		}

		public override string TypeOf() {
			return (UndefinedString);
		}

		public override bool CastToBoolean() {
			return (false);
		}

		public override int CastToInteger() {
			return (0);
		}

		public override double CastToFloat() {
			return (double.NaN);
		}

		public override string CastToString() {
			return (UndefinedString);
		}

		public override JSNumberValue ToNumber() {
			return (double.NaN);
		}
	}
}
