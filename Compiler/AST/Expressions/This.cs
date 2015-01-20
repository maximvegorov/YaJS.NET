using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class This : Expression {
		public This() : base(ExpressionType.This) {
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
