﻿using System.Collections.Generic;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors.Errors {
	/// <summary>
	/// Native-конструктор JSReferenceError
	/// </summary>
	internal sealed class JSReferenceErrorConstructor : JSNativeFunction {
		public JSReferenceErrorConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public override JSObject GetPrototype() {
			return (VM.ReferenceError);
		}

		public override JSValue Construct(ExecutionThread thread, LocalScope outerScope, List<JSValue> args) {
			return (VM.NewReferenceError(args.Count > 0 ? args[0].CastToString() : string.Empty));
		}

		public override JSValue Invoke(
			ExecutionThread thread,
			JSObject context,
			LocalScope outerScope,
			List<JSValue> args
			) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (1); } }
	}
}
