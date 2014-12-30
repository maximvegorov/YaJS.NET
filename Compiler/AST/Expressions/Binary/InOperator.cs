using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class InOperator : BinaryOperator {
		public InOperator(Expression leftOperand, Expression rightOperand)
			: base(leftOperand, rightOperand) {
			Contract.Requires(RightOperand.CanHaveMembers);
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand.ToString()).Append(" in ").Append(RightOperand.ToString());
			return (result.ToString());
		}
	}
}
