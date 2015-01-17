using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DecOperator : UnaryOperator {
		public DecOperator(Expression operand)
			: base(ExpressionType.Dec, operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return ("--" + Operand);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
