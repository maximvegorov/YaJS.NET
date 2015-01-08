namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Пустой оператор (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.1)
	/// </summary>
	internal sealed class EmptyStatement : Statement {
		public EmptyStatement(Statement parent)
			: base(parent, StatementType.Empty) {
		}
	}
}
