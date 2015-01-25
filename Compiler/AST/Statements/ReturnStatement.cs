using System.Diagnostics.Contracts;
using System.Text;

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

		public override string ToString() {
			var result = new StringBuilder();
			result.Append("return");
			if (Expression.Type != ExpressionType.UndefinedLiteral)
				result.Append(" ").Append(Expression);
			return (result.ToString());
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			for (var current = Parent; current != null; current = current.Parent)
				current.RegisterExitPoint(this);
		}

		public Expression Expression { get; private set; }
	}
}
