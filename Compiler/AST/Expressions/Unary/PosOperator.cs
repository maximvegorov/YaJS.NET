namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PosOperator : UnaryOperator {
		public PosOperator(Expression operand)
			: base(operand) {
		}

		public override string ToString() {
			return ("+" + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
