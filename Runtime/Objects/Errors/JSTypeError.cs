using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects.Errors {
	internal sealed class JSTypeError : JSError {
		public JSTypeError(VirtualMachine vm, string message, JSObject inherited)
			: base(vm, message, inherited) {
			Contract.Requires(inherited == vm.TypeError);
		}
	}
}
