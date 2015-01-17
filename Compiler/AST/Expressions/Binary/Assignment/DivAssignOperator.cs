using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DivAssignOperator : AssignOperator {
		public DivAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.DivAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" /= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
