using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DivOperator : BinaryOperator {
		public DivOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Div, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" / ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
