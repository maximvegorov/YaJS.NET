namespace YaJS.Runtime.Objects.Errors {
	internal sealed class JSSyntaxError : JSError {
		public JSSyntaxError(VirtualMachine vm, string message, JSObject inherited)
			: base(vm, message, inherited) {
		}
	}
}
