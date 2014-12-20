using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	internal sealed class JSInternalError : JSError {
		public JSInternalError(string message, string stackTrace, JSObject inherited)
			: base(message, inherited) {
			Contract.Requires(!string.IsNullOrEmpty(stackTrace));
			OwnMembers.Add("stackTrace", JSValue.Create(stackTrace));
		}
	}
}
