using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ConditionalOperator : Expression {
		private readonly Expression _condition;
		private readonly Expression _falseOperand;
		private readonly Expression _trueOperand;

		public ConditionalOperator(Expression condition, Expression trueOperand, Expression falseOperand)
			: base(ExpressionType.Conditional) {
			Contract.Requires(condition != null);
			Contract.Requires(trueOperand != null);
			Contract.Requires(falseOperand != null);
			_condition = condition;
			_trueOperand = trueOperand;
			_falseOperand = falseOperand;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(_condition)
				.Append('?')
				.Append(_trueOperand)
				.Append(':')
				.Append(_falseOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (_trueOperand.CanHaveMembers || _falseOperand.CanHaveMembers); } }

		public override bool CanHaveMutableMembers {
			get { return (_trueOperand.CanHaveMutableMembers || _falseOperand.CanHaveMutableMembers); }
		}

		public override bool CanBeConstructor {
			get { return (_trueOperand.CanBeConstructor || _falseOperand.CanBeConstructor); }
		}

		public override bool CanBeFunction { get { return (_trueOperand.CanBeFunction || _falseOperand.CanBeFunction); } }
		public override bool CanBeObject { get { return (_trueOperand.CanBeObject || _falseOperand.CanBeObject); } }

		public override bool IsConstant {
			get { return (_condition.IsConstant && _trueOperand.IsConstant && _falseOperand.IsConstant); }
		}
	}
}
