﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдоператор представляющий набор элементов выбора в операторе switch
	/// (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	public sealed class CaseClauseBlockStatement : LanguageStatement, IEnumerable<CaseClauseStatement> {
		private readonly List<CaseClauseStatement> _caseClauses;

		public CaseClauseBlockStatement(int lineNo)
			: base(StatementType.CaseClauseBlock, lineNo) {
			_caseClauses = new List<CaseClauseStatement>();
		}

		public void Add(CaseClauseStatement caseClause) {
			Contract.Requires(caseClause != null);
			if (caseClause.Parent != null)
				Errors.ThrowInternalError();
			caseClause.Parent = this;
			_caseClauses.Add(caseClause);
		}

		internal override void Preprocess(Function function) {
			foreach (var caseClause in _caseClauses)
				caseClause.Preprocess(function);
		}

		public bool IsEmpty { get { return (_caseClauses.Count == 0); } }

		#region IEnumerable<CaseClauseStatement>

		public IEnumerator<CaseClauseStatement> GetEnumerator() {
			return (_caseClauses.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return (_caseClauses.GetEnumerator());
		}

		#endregion
	}
}