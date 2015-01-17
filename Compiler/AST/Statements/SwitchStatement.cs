using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Элемент выбора оператора switch
	/// </summary>
	internal sealed class CaseClause {
		public CaseClause(Expression selector, IEnumerable<Statement> statements) {
			Contract.Requires(selector != null);
			Contract.Requires(statements != null);
			Selector = selector;
			Statements = statements;
		}

		public Expression Selector { get; private set; }
		public IEnumerable<Statement> Statements { get; private set; }
	}

	/// <summary>
	/// Оператор switch (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	internal sealed class SwitchStatement : LabellableStatement {
		private IEnumerable<CaseClause> _afterDefaultClauses;
		private IEnumerable<CaseClause> _beforeDefaultClauses;
		private IEnumerable<Statement> _defaultClause;
		private Expression _expression;

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

		public IEnumerable<Statement> DefaultClause {
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
