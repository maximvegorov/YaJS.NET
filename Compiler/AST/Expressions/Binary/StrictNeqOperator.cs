using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class StrictNeqOperator : BinaryOperator {
		public StrictNeqOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.StrictNeq, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" !== ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
