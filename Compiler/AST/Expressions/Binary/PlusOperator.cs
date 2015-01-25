using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PlusOperator : BinaryOperator {
		public PlusOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Or, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" + ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
