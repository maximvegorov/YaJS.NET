namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Пустой оператор (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.1)
	/// </summary>
	internal sealed class EmptyStatement : LanguageStatement {
		public EmptyStatement(Statement parent, int lineNo)
			: base(parent, StatementType.Empty, lineNo) {
		}
	}
}
