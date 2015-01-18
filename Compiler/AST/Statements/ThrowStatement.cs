using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор throw (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.13)
	/// </summary>
	public sealed class ThrowStatement : LanguageStatement {
		private readonly Expression _expression;

		public ThrowStatement(Statement parent, int lineNo, Expression expression)
			: base(parent, StatementType.Throw, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}

		public Expression Expression { get { return (_expression); } }
	}
}
