using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class InOperator : BinaryOperator {
		public InOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.In, leftOperand, rightOperand) {
			Contract.Requires(rightOperand.CanHaveMembers);
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" in ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
