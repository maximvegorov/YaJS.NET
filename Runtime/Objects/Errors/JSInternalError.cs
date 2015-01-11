using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	internal sealed class JSInternalError : JSError {
		public JSInternalError(VirtualMachine vm, string message, string stackTrace, JSObject inherited)
			: base(vm, message, inherited) {
			Contract.Requires(!string.IsNullOrEmpty(stackTrace));
			OwnMembers.Add("stackTrace", stackTrace);
		}
	}
}
