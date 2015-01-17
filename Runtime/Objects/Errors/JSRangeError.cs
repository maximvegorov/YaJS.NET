using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects.Errors {
	internal sealed class JSRangeError : JSError {
		public JSRangeError(VirtualMachine vm, string message, JSObject inherited)
			: base(vm, message, inherited) {
			Contract.Requires(inherited == vm.RangeError);
		}
	}
}
