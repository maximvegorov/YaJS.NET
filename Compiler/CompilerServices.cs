using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using YaJS.Compiler.AST;
using YaJS.Runtime;

namespace YaJS.Compiler {
	public sealed class CompilerServices : ICompilerServices {
		private static CompiledFunction Compile(Function function) {
			Contract.Requires(function != null);
			Contract.Ensures(Contract.Result<CompiledFunction>() != null);
			// Пока используем простую реализацию на основе рекурсии (потом заменим на обход на основе стека)
			if (function.NestedFunctions.Count == 0)
				return (FunctionCompiler.Compile(function, CompiledFunction.EmptyNestedFunctions));
			var nestedFunctions = new CompiledFunction[function.NestedFunctions.Count];
			for (var i = 0; i < nestedFunctions.Length; i++) {
				nestedFunctions[i] = Compile(function.NestedFunctions[i]);
				// Для того чтобы раньше освободить ссылку на функцию
				function.NestedFunctions[i] = null;
			}
			return (FunctionCompiler.Compile(function, nestedFunctions));
		}

		private static Function Parse(string functionName, IEnumerable<string> parameterNames, TextReader reader) {
			var parser = new Parser(new Tokenizer(reader));
			return (parser.ParseFunction(functionName, parameterNames));
		}

		private static Function ParseFile(string fileName) {
			using (var reader = File.OpenText(fileName))
				return (Parse(fileName, Enumerable.Empty<string>(), reader));
		}

		public static CompiledFunction Compile(TextReader reader) {
			Contract.Requires<ArgumentNullException>(reader != null, "reader");
			return (Compile(Parse("global", Enumerable.Empty<string>(), reader)));
		}

		public static CompiledFunction Compile(string fileName) {
			Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(fileName), "fileName");
			return (Compile(ParseFile(fileName)));
		}

		public CompiledFunction Compile(string functionName, IEnumerable<string> parameterNames, string functionBody) {
			return (Compile(Parse(functionName, parameterNames, new StringReader(functionBody))));
		}
	}
}
