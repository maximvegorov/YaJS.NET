using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ShlAssignOperator : AssignOperator {
		public ShlAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Shl, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" <<= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
