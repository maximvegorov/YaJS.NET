using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class Arguments : Expression {
		public const string ArgumentsLiteral = "arguments";

		internal Arguments()
			: base(ExpressionType.Arguments) {
		}

		public override string ToString() {
			return (ArgumentsLiteral);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdLocal, ArgumentsLiteral);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool CanBeObject {
			get { return (true); }
		}
	}
}
