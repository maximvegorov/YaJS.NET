using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Цикл (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6)
	/// </summary>
	public abstract class IterationStatement : LabellableStatement {
		private Statement _statement;

		protected IterationStatement(Statement parent, StatementType type, int lineNo, ILabelSet labelSet)
			: base(parent, type, lineNo, labelSet) {
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			if (!(_statement is CompoundStatement))
				_statement = _statement.WrapToBlock();
			_statement.InsertBefore(position, newStatement);
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
