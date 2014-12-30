namespace YaJS.Compiler.AST.Expressions {
	internal sealed class This : Expression {
		public override string ToString() {
			return ("this");
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
	}
}
