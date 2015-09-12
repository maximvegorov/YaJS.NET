using System;
using System.Linq;
using YaJS.Runtime;
using YaJS.Runtime.Objects;

namespace yajs.Objects {
	public partial class JSConsole {
		private sealed class Log : JSNativeFunction {
			public Log(VirtualMachine vm, JSObject inherited)
				: base(vm, inherited) {
			}

			public override JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
				switch (args.Length) {
					case 0:
						break;
					case 1:
						Console.Out.WriteLine(args[0].CastToString());
						break;
					default:
						Console.Out.WriteLine(args[0].CastToString(), args.Skip(1).Select(a => (object)a.CastToString()).ToArray());
						break;
				}
				return (Undefined);
			}

			public override int ParameterCount { get { return (1); } }
		}
	}
}
