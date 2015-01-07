namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NotOperator : UnaryOperator {
		public NotOperator(Expression operand)
			: base(operand) {
		}

		public override string ToString() {
			return ("!" + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
