using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор return (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.9)
	/// </summary>
	public sealed class ReturnStatement : LanguageStatement {
		public ReturnStatement(Statement parent, int lineNo, Expression expression)
			: base(parent, StatementType.Return, lineNo) {
			Contract.Requires(expression != null);
			Expression = expression;
		}

		public Expression Expression { get; private set; }
	}
}
