using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class IncOperator : UnaryOperator {
		public IncOperator(Expression operand)
			: base(operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return ("++" + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
