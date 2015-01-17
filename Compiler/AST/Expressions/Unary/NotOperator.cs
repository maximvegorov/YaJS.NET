namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NotOperator : UnaryOperator {
		public NotOperator(Expression operand)
			: base(ExpressionType.Not, operand) {
		}

		public override string ToString() {
			return ("!" + Operand);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
