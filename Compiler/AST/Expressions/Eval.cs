using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Eval : Expression {
		public const string EvalLiteral = "eval";

		public Eval() : base(ExpressionType.Eval) {
		}

		public override string ToString() {
			return (EvalLiteral);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdLocal, EvalLiteral);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
