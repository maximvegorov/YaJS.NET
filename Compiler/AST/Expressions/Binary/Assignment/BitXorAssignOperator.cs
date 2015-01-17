using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BitXorAssignOperator : AssignOperator {
		public BitXorAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.BitXorAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" ^= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
