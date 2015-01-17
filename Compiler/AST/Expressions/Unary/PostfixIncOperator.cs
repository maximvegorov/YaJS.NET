using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PostfixIncOperator : UnaryOperator {
		public PostfixIncOperator(Expression operand)
			: base(ExpressionType.PostfixInc, operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return (Operand + "++");
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
