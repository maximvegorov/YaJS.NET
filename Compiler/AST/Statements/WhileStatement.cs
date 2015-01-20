using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор while (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.2)
	/// </summary>
	public sealed class WhileStatement : IterationStatement {
		private Expression _condition;

		public WhileStatement(int lineNo, ILabelSet labelSet)
			: base(StatementType.While, lineNo, labelSet) {
		}

		public Expression Condition {
			get { return (_condition); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_condition == null);
				_condition = value;
			}
		}
	}
}
