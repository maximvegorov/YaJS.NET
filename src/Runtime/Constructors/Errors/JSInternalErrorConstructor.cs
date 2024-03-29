﻿using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors.Errors {
	/// <summary>
	/// Native-конструктор JSInternalError
	/// </summary>
	internal sealed class JSInternalErrorConstructor : JSNativeFunction {
		public JSInternalErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.InternalError);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			return (VM.NewInternalError(
				args.Length > 0 ? args[0].CastToString() : string.Empty,
				args.Length > 1 ? args[1].CastToString() : "Unknown"));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (2); } }
	}
}
