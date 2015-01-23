using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class This : Expression {
		internal This() : base(ExpressionType.This) {
		}

		public override string ToString() {
			return ("this");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdThis);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
