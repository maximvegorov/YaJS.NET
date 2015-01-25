namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор представляющий тело функции
	/// </summary>
	public sealed class FunctionBodyStatement : CompoundStatement {
		public FunctionBodyStatement()
			: base(1) {
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			base.Preprocess(compiler);
			if (Statements.Count == 0)
				Add(new ReturnStatement(compiler.Function.LineNo, Expression.Undefined()));
			else if (Statements[Statements.Count - 1].Type != StatementType.Return)
				Add(new ReturnStatement(Statements[Statements.Count - 1].LineNo, Expression.Undefined()));
		}
	}
}
