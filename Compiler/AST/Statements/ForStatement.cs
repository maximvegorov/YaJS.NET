using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.3)
	/// </summary>
	internal sealed class ForStatement : IterationStatement {
		private Expression _initialization;
		private Expression _condition;
		private Expression _increment;

		public ForStatement(
			Statement parent,
			Expression initialization,
			Expression condition,
			Expression increment,
			ILabelSet labelSet
		) : base(parent, StatementType.For, labelSet) {
			_initialization = initialization;
			_condition = condition;
			_increment = increment;
		}
	}
}
