using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class UndefinedLiteral : Expression {
		internal UndefinedLiteral()
			: base(ExpressionType.UndefinedLiteral) {
		}

		public override string ToString() {
			return ("undefined");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdUndefined);
		}

		public override bool IsConstant { get { return (true); } }
	}
}
