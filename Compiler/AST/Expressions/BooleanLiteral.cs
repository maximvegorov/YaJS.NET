namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BooleanLiteral : Expression {
		public BooleanLiteral(bool value) {
			Value = value;
		}

		public override string ToString() {
			return (Value ? "true" : "false");
		}

		public bool Value { get; private set; }
	}
}
