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
			SwitchJumpTables = new List<SwitchJumpTable>();
			StatementStarts = new Dictionary<Statement, Label>();
			StatementEnds = new Dictionary<Statement, Label>();
		}

		private CompiledFunction Compile(CompiledFunction[] nestedFunctions) {
			Function.ProcessTryFinallyStatements();
			Function.FunctionBody.CompileBy(this);
			return (new CompiledFunction(
				Function.Name,
				Function.LineNo,
				Function.ParameterNames.Count == 0
					? CompiledFunction.EmptyParameterNames
					: Function.ParameterNames.ToArray(),
				Function.DeclaredVariables.Count == 0
					? CompiledFunction.EmptyDeclaredVariables
					: Function.DeclaredVariables.ToArray(),
				nestedFunctions,
				Function.FunctionDeclarationCount,
				Function.FunctionBody.ToString(),
				Emitter.ToCompiledCode(),
				SwitchJumpTables.Count == 0
					? CompiledFunction.EmptySwitchJumpTables
					: SwitchJumpTables.ToArray()
				));
		}

		public static CompiledFunction Compile(Function function, CompiledFunction[] nestedFunctions) {
			Contract.Requires(function != null);
			Contract.Requires(nestedFunctions != null && nestedFunctions.Length == function.NestedFunctions.Count);
			var compiler = new FunctionCompiler(function);
			return (compiler.Compile(nestedFunctions));
		}

		public Function Function { get; private set; }
		public ByteCodeEmitter Emitter { get; private set; }
		public List<SwitchJumpTable> SwitchJumpTables { get; private set; }
		public Dictionary<Statement, Label> StatementStarts { get; private set; }
		public Dictionary<Statement, Label> StatementEnds { get; private set; }
	}
}
