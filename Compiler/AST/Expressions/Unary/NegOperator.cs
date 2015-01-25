namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NegOperator : UnaryOperator {
		public NegOperator(Expression operand)
			: base(ExpressionType.Neg, operand) {
		}

		public override string ToString() {
			return ("-" + Operand);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
