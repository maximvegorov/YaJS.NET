using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class MinusAssignOperator : AssignOperator {
		public MinusAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.MinusAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" -= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
