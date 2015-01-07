namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Eval : Expression {
		public override string ToString() {
			return ("eval");
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
	}
}
