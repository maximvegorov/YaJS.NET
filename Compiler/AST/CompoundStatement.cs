using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Базовый класс для всех составных операторов
	/// </summary>
	public abstract class CompoundStatement : Statement, IEnumerable<Statement> {
		private List<Statement> _statements;

		public CompoundStatement(Statement parent, StatementType type)
			: base(parent, type) {
			_statements = new List<Statement>();
		}

		public void AddStatement(Statement statement) {
			Contract.Requires(statement != null && statement.Parent == this);
			_statements.Add(statement);
		}

		#region IEnumerable<Statement>

		public IEnumerator<Statement> GetEnumerator() {
			return (_statements.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return (_statements.GetEnumerator());
		}

		#endregion
	}
}
