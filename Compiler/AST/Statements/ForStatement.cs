namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.3)
	/// </summary>
	public sealed class ForStatement : IterationStatement {
		private readonly Expression _initialization;
		private readonly Expression _condition;
		private readonly Expression _increment;

		internal ForStatement(
			int lineNo,
			Expression initialization,
			Expression condition,
			Expression increment,
			ILabelSet labelSet
			) : base(StatementType.For, lineNo, labelSet) {
			_initialization = initialization;
			_condition = condition;
			_increment = increment;
		}

		public Expression Initialization { get { return (_initialization); } }
		public Expression Condition { get { return (_condition); } }
		public Expression Increment { get { return (_increment); } }
	}
}
