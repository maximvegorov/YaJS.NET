namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NullLiteral : Expression {
		public NullLiteral() : base(ExpressionType.Null) {
		}

		public override string ToString() {
			return ("null");
		}

		public override bool IsConstant { get { return (true); } }
	}
}
