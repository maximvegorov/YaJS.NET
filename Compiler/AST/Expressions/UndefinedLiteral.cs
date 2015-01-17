namespace YaJS.Compiler.AST.Expressions {
	internal sealed class UndefinedLiteral : Expression {
		public UndefinedLiteral()
			: base(ExpressionType.Undefined) {
		}

		public override string ToString() {
			return ("undefined");
		}

		public override bool IsConstant { get { return (true); } }
	}
}
