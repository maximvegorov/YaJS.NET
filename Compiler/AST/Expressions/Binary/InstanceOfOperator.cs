using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class InstanceOfOperator : BinaryOperator {
		public InstanceOfOperator(Expression leftOperand, Expression rightOperand)
			: base(leftOperand, rightOperand) {
			Contract.Requires(rightOperand.CanBeConstructor);
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand.ToString()).Append(" instanceof ").Append(RightOperand.ToString());
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
