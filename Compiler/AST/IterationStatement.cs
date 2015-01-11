using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Цикл (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6)
	/// </summary>
	internal abstract class IterationStatement : LabellableStatement {
		private Statement _statement;

		protected IterationStatement(Statement parent, StatementType type, int lineNo, ILabelSet labelSet)
			: base(parent, type, lineNo, labelSet) {
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
