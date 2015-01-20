namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Пустой оператор (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.1)
	/// </summary>
	public sealed class EmptyStatement : LanguageStatement {
		public EmptyStatement(int lineNo)
			: base(StatementType.Empty, lineNo) {
		}
	}
}
