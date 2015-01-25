using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Constructors {
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

		public override JSValue Construct(ExecutionThread thread, LocalScope outerScope, List<JSValue> args) {
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
			return (VM.NewFunction(outerScope, VM.Compiler.Compile("f", parameterNames, functionBody)));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, LocalScope outerScope, List<JSValue> args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount {
			get { return (1); }
		}
	}
}
