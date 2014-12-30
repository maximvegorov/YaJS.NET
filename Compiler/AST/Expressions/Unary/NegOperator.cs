namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NegOperator : UnaryOperator {
		public NegOperator(Expression operand)
			: base(operand) {
		}

		public override string ToString() {
			return ("-" + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
