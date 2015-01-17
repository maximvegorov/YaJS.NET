namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Arguments : Expression {
		public Arguments() : base(ExpressionType.Arguments) {
		}

		public override string ToString() {
			return ("arguments");
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
