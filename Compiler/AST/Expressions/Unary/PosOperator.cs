namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PosOperator : UnaryOperator {
		public PosOperator(Expression operand)
			: base(ExpressionType.Pos, operand) {
		}

		public override string ToString() {
			return ("+" + Operand);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
