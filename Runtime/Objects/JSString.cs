using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект-wrapper для строковых значений
	/// </summary>
	public sealed class JSString : JSObject {
		internal JSString(string value, JSObject inherited)
			: base(inherited) {
			Contract.Requires(inherited != null);
			Value = value ?? string.Empty;
		}

		public override string ToString() {
			return (Value);
		}

		public string Value { get; private set; }
	}
}
