using System.Diagnostics.Contracts;
using System.Globalization;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект-wrapper для числовых значений
	/// </summary>
	public sealed class JSNumber : JSObject {
		internal JSNumber(double value, JSObject inherited)
			: base(inherited) {
			Contract.Requires(inherited != null);
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		public double Value { get; private set; }
	}
}
