namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.3)
	/// </summary>
	internal sealed class ForStatement : IterationStatement {
		private readonly Expression _condition;
		private readonly Expression _increment;
		private readonly Expression _initialization;

		public ForStatement(
			Statement parent,
			int lineNo,
			Expression initialization,
			Expression condition,
			Expression increment,
			ILabelSet labelSet
			) : base(parent, StatementType.For, lineNo, labelSet) {
			_initialization = initialization;
			_condition = condition;
			_increment = increment;
		}
	}
}
