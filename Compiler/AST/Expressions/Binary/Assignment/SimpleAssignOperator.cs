using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class SimpleAssignOperator : AssignOperator {
		public SimpleAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.SimpleAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" = ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanBeObject { get { return (RightOperand.CanBeObject); } }
	}
}
