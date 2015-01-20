using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор switch (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	public sealed class SwitchStatement : LabellableStatement {
		private Expression _expression;
		private CaseClauseBlockStatement _beforeDefault;
		private StatementListStatement _defaultClause;
		private CaseClauseBlockStatement _afterDefault;

		public SwitchStatement(int lineNo, ILabelSet labelSet)
			: base(StatementType.Switch, lineNo, labelSet) {
		}

		internal override void Preprocess(Function function) {
			if (_beforeDefault == null)
				Errors.ThrowInternalError();
			else
				_beforeDefault.Preprocess(function);
			if (_defaultClause != null)
				_defaultClause.Preprocess(function);
			if (_afterDefault != null)
				_afterDefault.Preprocess(function);
		}

		public Expression Expression {
			set {
				Contract.Requires(value != null);
				Contract.Assert(_expression == null);
				_expression = value;
			}
		}

		public CaseClauseBlockStatement BeforeDefault {
			get { return (_beforeDefault); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_beforeDefault == null);
				if (value.Parent != null)
					Errors.ThrowInternalError();
				value.Parent = this;
				_beforeDefault = value;
			}
		}

		public StatementListStatement DefaultClause {
			get { return (_defaultClause); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_defaultClause == null);
				if (value.Parent != null)
					value.Remove();
				value.Parent = this;
				_defaultClause = value;
			}
		}

		public CaseClauseBlockStatement AfterDefault {
			get { return (_afterDefault); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_afterDefault == null);
				if (value.Parent != null)
					Errors.ThrowInternalError();
				value.Parent = this;
				_afterDefault = value;
			}
		}
	}
}
