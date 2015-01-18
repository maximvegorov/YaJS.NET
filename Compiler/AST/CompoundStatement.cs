using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Базовый класс для всех составных операторов
	/// </summary>
	public abstract class CompoundStatement : LanguageStatement, IEnumerable<Statement> {
		private readonly List<Statement> _statements;

		protected CompoundStatement(Statement parent, StatementType type, int lineNo)
			: base(parent, type, lineNo) {
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
