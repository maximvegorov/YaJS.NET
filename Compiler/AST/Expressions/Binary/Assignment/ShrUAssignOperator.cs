using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ShrUAssignOperator : AssignOperator {
		public ShrUAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.ShrUAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" >>>= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
