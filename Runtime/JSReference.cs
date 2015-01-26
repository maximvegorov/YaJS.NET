using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime {
	public sealed class JSReference : JSValue {
		public JSReference(JSObject baseValue, JSValue property)
			: base(JSValueType.Reference) {
			Contract.Requires(baseValue != null);
			Contract.Requires(property != null);
			BaseValue = baseValue;
			Property = property;
		}

		internal override JSReference RequireReference() {
			return (this);
		}

		public JSObject BaseValue { get; private set; }
		public JSValue Property { get; private set; }

		public JSValue Value {
			get { return (BaseValue.GetMember(Property)); }
			set { BaseValue.SetMember(Property, value); }
		}
	}
}
