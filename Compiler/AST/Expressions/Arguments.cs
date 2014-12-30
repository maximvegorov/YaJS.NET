namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Arguments : Expression {
		public override string ToString() {
			return ("arguments");
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
