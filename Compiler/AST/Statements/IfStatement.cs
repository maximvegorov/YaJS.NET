using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор if (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.5)
	/// </summary>
	internal sealed class IfStatement : Statement {
		private Expression _condition;
		private Statement _thenStatement;
		private Statement _elseStatement;

		public IfStatement(Statement parent)
			: base(parent, StatementType.If) {
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
