using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Базовый класс для всех составных операторов
	/// </summary>
	public abstract class CompoundStatement : Statement {
		private List<Statement> _statements;

		public CompoundStatement(Statement parent, StatementType type)
			: base(parent, type, Statement.EmptyLabelSet) {
			_statements = new List<Statement>();
		}

		public void AddStatement(Statement statement) {
			Contract.Requires(statement != null && statement.Parent == this);
			_statements.Add(statement);
		}
	}
}
