using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор return (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.9)
	/// </summary>
	internal sealed class ReturnStatement : Statement {
		private Expression _expression;

		public ReturnStatement(Statement parent, int lineNo, Expression expression)
			: base(parent, StatementType.Return, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}
	}
}
