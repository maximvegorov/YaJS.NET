using System;
using yajs.Objects;
using YaJS.Compiler;
using YaJS.Runtime;

namespace yajs {
	public static class Program {
		static int Main(string[] args) {
			try {
				if (args.Length == 0) {
					Console.WriteLine("Usage: yajs filename.js");
					return (-1);
				}
				var vm = new VirtualMachine(new CompilerServices());
				vm.GlobalObject.OwnMembers.Add("console", new JSConsole(vm));
				var program = CompilerServices.Compile(args[0]);
				var exitCode = vm.Execute(program);
				return (exitCode.CastToInteger());
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return (-1);
			}
		}
	}
}
