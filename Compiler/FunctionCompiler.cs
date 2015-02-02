using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Statements;
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
			TryStatements = new List<TryStatement>();
		}

		private void ProcessTryStatements() {
			// Копируем операторы блока finally перед каждой точкой выхода из блока try
			foreach (var tryStatement in TryStatements) {
				if (tryStatement.FinallyBlock == null || tryStatement.TryBlock.ExitPoints.Count == 0)
					continue;
				var finallyBlock = tryStatement.FinallyBlock;
				foreach (var exitPoint in tryStatement.TryBlock.ExitPoints)
					exitPoint.InsertBefore(new ReferenceStatement(finallyBlock));
			}
		}

		private CompiledFunction Compile(CompiledFunction[] nestedFunctions) {
			Function.Preprocess(this);
			ProcessTryStatements();
			Function.CompileBy(this);
			return
				(new CompiledFunction(
					Function.Name,
					Function.LineNo,
					Function.ParameterNames.Count == 0 ? CompiledFunction.EmptyParameterNames : Function.ParameterNames.ToArray(),
					Function.DeclaredVariables.Count == 0
						? CompiledFunction.EmptyDeclaredVariables
						: Function.DeclaredVariables.ToArray(),
					nestedFunctions,
					Function.FunctionDeclarationCount,
					Function.FunctionBody.ToString(),
					Emitter.ToCompiledCode(),
					SwitchJumpTables.Count == 0 ? CompiledFunction.EmptySwitchJumpTables : SwitchJumpTables.ToArray()));
		}

		[Conditional("DEBUG")]
		internal void MarkEndOfStatement() {
			Emitter.Emit(OpCode.Break);
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
		public List<TryStatement> TryStatements { get; private set; }
	}
}
