using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects.Errors {
	internal sealed class JSInternalError : JSError {
		public JSInternalError(VirtualMachine vm, string message, string stackTrace, JSObject inherited)
			: base(vm, message, inherited) {
			Contract.Requires(!string.IsNullOrEmpty(stackTrace));
			Contract.Requires(inherited == vm.InternalError);
			OwnMembers.Add("stackTrace", stackTrace);
		}
	}
}
