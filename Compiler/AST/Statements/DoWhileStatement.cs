using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор do-while (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.1)
	/// </summary>
	internal sealed class DoWhileStatement : IterationStatement {
		private Expression _condition;

		public DoWhileStatement(Statement parent, int lineNo, ILabelSet labelSet)
			: base(parent, StatementType.DoWhile, lineNo, labelSet) {
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
