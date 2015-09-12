namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор представляющий простой список операторов
	/// </summary>
	public sealed class StatementListStatement : CompoundStatement {
		public StatementListStatement(int lineNo)
			: base(lineNo) {
		}
	}
}
