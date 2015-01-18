using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор if (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.5)
	/// </summary>
	public sealed class IfStatement : LanguageStatement {
		private Expression _condition;
		private Statement _thenStatement;
		private Statement _elseStatement;

		public IfStatement(Statement parent, int lineNo)
			: base(parent, StatementType.If, lineNo) {
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			Statement target;
			if (ReferenceEquals(position, _thenStatement)) {
				if (_thenStatement.Type != StatementType.Compound)
					_thenStatement = _thenStatement.WrapToBlock();
				target = _thenStatement;
			}
			else {
				Contract.Assert(ReferenceEquals(position, _elseStatement));
				if (_elseStatement.Type != StatementType.Compound)
					_elseStatement = _elseStatement.WrapToBlock();
				target = _elseStatement;
			}
			target.InsertBefore(position, newStatement);
		}

		public Expression Condition {
			get { return (_condition); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_condition == null);
				_condition = value;
			}
		}

		public Statement ThenStatement {
			get { return (_thenStatement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_thenStatement == null);
				_thenStatement = value;
			}
		}

		public Statement ElseStatement {
			get { return (_elseStatement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_elseStatement == null);
				_elseStatement = value;
			}
		}
	}
}
