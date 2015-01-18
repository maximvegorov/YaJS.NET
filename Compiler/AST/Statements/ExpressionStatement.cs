using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор выражение (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.4)
	/// </summary>
	public sealed class ExpressionStatement : LanguageStatement {
		private readonly Expression _expression;

		public ExpressionStatement(Statement parent, int lineNo, Expression expression)
			: base(parent, StatementType.Expression, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}

		public Expression Expression { get { return (_expression); } }
	}
}
