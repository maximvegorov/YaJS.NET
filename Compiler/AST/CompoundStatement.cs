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
			// Пропускаем сразу пустые операторы
			if (statement.Type != StatementType.Empty)
				_statements.Add(statement);
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			var index = _statements.IndexOf(position);
			Contract.Assert(index != -1);
			newStatement.Parent = this;
			_statements.Insert(index, newStatement);
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			foreach (var statement in _statements)
				statement.CompileBy(compiler);
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
