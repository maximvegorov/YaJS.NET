using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Элемент выбора оператора switch
	/// </summary>
	public sealed class CaseClause {
		public CaseClause(Expression selector, SwitchClauseStatement statement) {
			Contract.Requires(selector != null);
			Contract.Requires(statement != null);
			Selector = selector;
			Statement = statement;
		}

		public Expression Selector { get; private set; }
		public SwitchClauseStatement Statement { get; private set; }
	}

	/// <summary>
	/// Оператор switch (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	public sealed class SwitchStatement : LabellableStatement {
		private Expression _expression;
		private IEnumerable<CaseClause> _beforeDefaultClauses;
		private SwitchClauseStatement _defaultClause;
		private IEnumerable<CaseClause> _afterDefaultClauses;

		public SwitchStatement(Statement parent, int lineNo, ILabelSet labelSet)
			: base(parent, StatementType.Switch, lineNo, labelSet) {
		}

		public Expression Expression {
			set {
				Contract.Requires(value != null);
				Contract.Assert(_expression == null);
				_expression = value;
			}
		}

		public IEnumerable<CaseClause> BeforeDefaultClauses {
			set {
				Contract.Requires(value != null);
				Contract.Assert(_beforeDefaultClauses == null);
				_beforeDefaultClauses = value;
			}
		}

		public SwitchClauseStatement DefaultClause {
			set {
				Contract.Requires(value != null);
				Contract.Assert(_defaultClause == null);
				_defaultClause = value;
			}
		}

		public IEnumerable<CaseClause> AfterDefaultClauses {
			set {
				Contract.Requires(value != null);
				Contract.Assert(_afterDefaultClauses == null);
				_afterDefaultClauses = value;
			}
		}
	}
}
