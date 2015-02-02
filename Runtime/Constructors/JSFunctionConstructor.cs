using System.Collections.Generic;
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

		public override JSObject GetPrototype() {
			return (VM.Function);
		}

		public override JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			IEnumerable<string> parameterNames;
			string functionBody;
			if (args.Length == 0) {
				parameterNames = Enumerable.Empty<string>();
				functionBody = string.Empty;
			}
			else {
				parameterNames = args.Take(args.Length - 1).Select(arg => arg.CastToString());
				functionBody = args[args.Length - 1].CastToString();
			}
			return (VM.NewFunction(outerScope, VM.Compiler.Compile("f", parameterNames, functionBody)));
		}

		public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			return (Construct(thread, outerScope, args));
		}

		public override int ParameterCount { get { return (1); } }
	}
}
