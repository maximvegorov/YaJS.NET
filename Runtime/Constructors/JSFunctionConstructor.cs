using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Runtime.Constructors {
	using Runtime.Objects;

	/// <summary>
	/// Native-конструктор JSFunction
	/// </summary>
	internal sealed class JSFunctionConstructor : JSNativeFunction {
		public JSFunctionConstructor(JSObject inherited)
			: base(inherited) {
		}

		public static void InitPrototype(JSObject proto) {
			Contract.Requires(proto != null);
			// TODO
		}

		public override JSObject GetPrototype(VirtualMachine vm) {
			return (vm.Function);
		}

		public override JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			IEnumerable<string> argumentNames;
			string functionBody;
			if (args.Count == 0) {
				argumentNames = Enumerable.Empty<string>();
				functionBody = string.Empty;
			}
			else {
				argumentNames = args.Take(args.Count - 1).Select(arg => arg.CastToString());
				functionBody = args[args.Count - 1].CastToString();
			}
			return (vm.NewFunction(
				vm.Compiler.Compile(argumentNames, functionBody), outerScope
			));
		}
	}
}
