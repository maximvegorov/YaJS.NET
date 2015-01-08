using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор while (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.2)
	/// </summary>
	internal sealed class WhileStatement : LabellableStatement {
		private Expression _condition;
		private Statement _statement;

		public WhileStatement(Statement parent, ILabelSet labelSet)
			: base(parent, StatementType.While, labelSet) {
		}

		public Expression Condition {
			get { return (_condition); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_condition == null);
				_condition = value;
			}
		}
		public Statement Statement {
			get { return (_statement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_statement == null);
				_statement = value;
			}
		}
	}
}
