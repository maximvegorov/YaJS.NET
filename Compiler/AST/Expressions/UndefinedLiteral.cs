using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class UndefinedLiteral : Expression {
		public UndefinedLiteral()
			: base(ExpressionType.Undefined) {
		}

		public override string ToString() {
			return ("undefined");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdUndefined);
		}

		public override bool IsConstant { get { return (true); } }
	}
}
