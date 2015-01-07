namespace YaJS.Compiler.AST.Expressions {
	internal sealed class UndefinedLiteral : Expression {
		public override string ToString() {
			return ("undefined");
		}

		public override bool IsConstant { get { return (true); } }
	}
}
