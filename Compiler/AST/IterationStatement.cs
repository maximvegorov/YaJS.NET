using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Цикл (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6)
	/// </summary>
	public abstract class IterationStatement : LabellableStatement {
		private Statement _statement;

		protected IterationStatement(StatementType type, int lineNo, ILabelSet labelSet)
			: base(type, lineNo, labelSet) {
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			if (_statement.Type != StatementType.Compound)
				_statement = _statement.WrapToBlock();
			_statement.InsertBefore(position, newStatement);
		}

		protected override void Remove(Statement child) {
			if (ReferenceEquals(_statement, child))
				_statement = null;
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			if (_statement == null)
				Errors.ThrowInternalError();
			else
				_statement.Preprocess(compiler);
		}

		public Statement Statement {
			get { return (_statement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_statement == null);
				if (value.Parent != null)
					value.Parent.Remove();
				value.Parent = this;
				_statement = value;
			}
		}
	}
}
