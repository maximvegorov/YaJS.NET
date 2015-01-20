using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Arguments : Expression {
		public const string ArgumentsLiteral = "arguments";

		public Arguments() : base(ExpressionType.Arguments) {
		}

		public override string ToString() {
			return (ArgumentsLiteral);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdLocal, ArgumentsLiteral);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
