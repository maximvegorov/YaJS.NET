﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Базовый класс для всех составных операторов
	/// </summary>
	public abstract class CompoundStatement : LanguageStatement, IEnumerable<Statement> {
		private readonly List<Statement> _statements;

		protected CompoundStatement(int lineNo)
			: base(StatementType.Compound, lineNo) {
			_statements = new List<Statement>();
		}

		public void Append(Statement statement) {
			Contract.Requires(statement != null);
			// Игнорируем пустые операторы
			if (statement.Type == StatementType.Empty)
				return;
			if (statement.Parent != null)
				statement.Remove();
			statement.Parent = this;
			_statements.Add(statement);
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			var index = _statements.IndexOf(position);
			Contract.Assert(index != -1);
			if (newStatement.Parent != null)
				newStatement.Remove();
			else
				newStatement.Parent = this;
			_statements.Insert(index, newStatement);
		}

		protected override void Remove(Statement child) {
			_statements.Remove(child);
		}

		internal override void Preprocess(Function function) {
			foreach (var statement in _statements)
				statement.Preprocess(function);
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
