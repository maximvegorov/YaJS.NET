using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ModAssignOperator : AssignOperator {
		public ModAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.ModAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" %= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
