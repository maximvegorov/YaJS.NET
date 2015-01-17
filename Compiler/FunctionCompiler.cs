using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;
using YaJS.Compiler.Emitter;
using YaJS.Runtime;

namespace YaJS.Compiler {
	/// <summary>
	/// Преобразует FunctionBody в байт-код
	/// </summary>
	internal sealed class FunctionCompiler {
		private FunctionCompiler(Function function) {
			Function = function;
			Emitter = new ByteCodeEmitter();
			StatementStarts = new Dictionary<Statement, Label>();
			StatementEnds = new Dictionary<Statement, Label>();
		}

		private CompiledFunction Compile(CompiledFunction[] nestedFunctions) {
			throw new NotImplementedException();
		}

		public static CompiledFunction Compile(Function function, CompiledFunction[] nestedFunctions) {
			Contract.Requires(function != null);
			Contract.Requires(nestedFunctions != null);
			var compiler = new FunctionCompiler(function);
			return (compiler.Compile(nestedFunctions));
		}

		public Function Function { get; private set; }
		public ByteCodeEmitter Emitter { get; private set; }
		public Dictionary<Statement, Label> StatementStarts { get; private set; }
		public Dictionary<Statement, Label> StatementEnds { get; private set; }
	}
}
