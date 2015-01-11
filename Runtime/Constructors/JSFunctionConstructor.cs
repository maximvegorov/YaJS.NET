using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSFunction
	/// </summary>
	internal sealed class JSFunctionConstructor : JSNativeFunction {
		public JSFunctionConstructor(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		public static void InitPrototype(JSObject proto) {
			Contract.Requires(proto != null);
			// TODO
		}

		public override JSObject GetPrototype() {
			return (VM.Function);
		}

		public override JSValue Construct(LocalScope outerScope, List<JSValue> args) {
			IEnumerable<string> parameterNames;
			string functionBody;
			if (args.Count == 0) {
				parameterNames = Enumerable.Empty<string>();
				functionBody = string.Empty;
			}
			else {
				parameterNames = args.Take(args.Count - 1).Select(arg => arg.CastToString());
				functionBody = args[args.Count - 1].CastToString();
			}
			return (VM.NewFunction(
				outerScope, VM.Compiler.Compile("f", parameterNames, functionBody)
			));
		}

		public override JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (Construct(outerScope, args));
		}
	}
}
