using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class LtOperator : BinaryOperator {
		public LtOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Lt, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" < ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
