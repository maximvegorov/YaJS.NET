using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор представляющий тело функции
	/// </summary>
	public sealed class FunctionBodyStatement : CompoundStatement {
		public FunctionBodyStatement()
			: base(1) {
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			// В режиме eval в стеке вычислений должно всегда быть хотя бы одно значение
			// Всегда загружаем изначально undefined чтобы упростить последующую кодогенерацию
			if (compiler.IsEvalMode)
				compiler.Emitter.Emit(OpCode.LdUndefined);

			base.CompileBy(compiler);

			// Необходим гарантированный возврат из функции
			if (Statements.Count == 0 || Statements[Statements.Count - 1].Type != StatementType.Return) {
				// В случае режима eval результат уже в стеке вычислений
				if (!compiler.IsEvalMode)
					compiler.Emitter.Emit(OpCode.LdUndefined);
				compiler.Emitter.Emit(OpCode.Return);
			}
		}
	}
}
