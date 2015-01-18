namespace YaJS.Compiler.AST {
	/// <summary>
	/// Псевдооператор. Используется в качестве тела функции
	/// </summary>
	internal sealed class FunctionBody : CompoundStatement {
		public FunctionBody()
			: base(null, StatementType.Compound, 1) {
		}
	}
}
