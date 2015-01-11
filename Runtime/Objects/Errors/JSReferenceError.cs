﻿namespace YaJS.Runtime.Objects.Errors {
	internal sealed class JSReferenceError : JSError {
		public JSReferenceError(VirtualMachine vm, string message, JSObject inherited)
			: base(vm, message, inherited) {
		}
	}
}
