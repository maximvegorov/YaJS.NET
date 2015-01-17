using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BitOrAssignOperator : AssignOperator {
		public BitOrAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.BitOrAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" |= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
