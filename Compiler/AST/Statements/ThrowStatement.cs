using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор throw (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.13)
	/// </summary>
	internal sealed class ThrowStatement : Statement {
		private Expression _expression;

		public ThrowStatement(Statement parent, int lineNo, Expression expression)
			: base(parent, StatementType.Throw, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}
	}
}
