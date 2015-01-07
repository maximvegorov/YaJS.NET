using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ConditionalOperator : Expression {
		public ConditionalOperator(Expression condition, Expression trueOperand, Expression falseOperand) {
			Contract.Requires(condition != null);
			Contract.Requires(trueOperand != null);
			Contract.Requires(falseOperand != null);
			Condition = condition;
			TrueOperand = trueOperand;
			FalseOperand = falseOperand;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(Condition.ToString())
				.Append('?')
				.Append(TrueOperand.ToString())
				.Append(':')
				.Append(FalseOperand.ToString());
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (TrueOperand.CanHaveMembers || FalseOperand.CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (TrueOperand.CanHaveMutableMembers || FalseOperand.CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (TrueOperand.CanBeConstructor || FalseOperand.CanBeConstructor); } }
		public override bool CanBeFunction { get { return (TrueOperand.CanBeFunction || FalseOperand.CanBeFunction); } }
		public override bool IsConstant { get { return (Condition.IsConstant && TrueOperand.IsConstant && FalseOperand.IsConstant); } }

		public Expression Condition { get; private set; }
		public Expression TrueOperand { get; private set; }
		public Expression FalseOperand { get; private set; }
	}
}
