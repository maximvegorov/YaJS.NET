using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект-wrapper для логических значений
	/// </summary>
	public sealed class JSBoolean : JSObject {
		internal JSBoolean(bool value, JSObject inherited)
			: base(inherited) {
				Contract.Requires(inherited != null);
		}

		public override string ToString() {
			return (Value.ToString());
		}

		public bool Value { get; private set; }
	}
}
