using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PlusAssignOperator : AssignOperator {
		public PlusAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.PlusAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" += ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
