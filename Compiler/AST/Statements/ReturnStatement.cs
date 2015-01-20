using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор return (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.9)
	/// </summary>
	public sealed class ReturnStatement : LanguageStatement {
		public ReturnStatement(int lineNo, Expression expression)
			: base(StatementType.Return, lineNo) {
			Contract.Requires(expression != null);
			Expression = expression;
		}

		internal override void Preprocess(Function function) {
			for (var current = Parent; current != null; current = current.Parent)
				current.RegisterExitPoint(this);
		}

		public Expression Expression { get; private set; }
	}
}
