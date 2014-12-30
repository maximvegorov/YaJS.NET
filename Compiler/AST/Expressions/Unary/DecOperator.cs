using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DecOperator : UnaryOperator {
		public DecOperator(Expression operand)
			: base(operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return ("--" + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
