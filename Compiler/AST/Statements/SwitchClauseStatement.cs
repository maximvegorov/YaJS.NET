namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор представляющий набор операторов связанных c CaseClause или DefaultClause оператора switch
	/// (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	public sealed class SwitchClauseStatement : CompoundStatement {
		public SwitchClauseStatement(Statement parent, int lineNo)
			: base(parent, StatementType.Compound, lineNo) {
		}
	}
}
