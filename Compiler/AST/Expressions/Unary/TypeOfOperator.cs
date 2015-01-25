namespace YaJS.Compiler.AST.Expressions {
	internal sealed class TypeOfOperator : UnaryOperator {
		public TypeOfOperator(Expression operand)
			: base(ExpressionType.TypeOf, operand) {
		}

		public override string ToString() {
			return ("typeof " + Operand);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
