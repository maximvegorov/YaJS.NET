using System;
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
				var program = CompilerServices.Compile(args[1]);
				return (vm.Execute(program).CastToInteger());
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return (-1);
			}
		}
	}
}
