namespace YaJS.Compiler.AST.Expressions {
	internal sealed class TypeOfOperator : UnaryOperator {
		public TypeOfOperator(Expression operand)
			: base(operand) {
		}

		public override string ToString() {
			return ("typeof " + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
