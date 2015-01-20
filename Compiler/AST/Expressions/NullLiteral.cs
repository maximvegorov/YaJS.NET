using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NullLiteral : Expression {
		public NullLiteral() : base(ExpressionType.Null) {
		}

		public override string ToString() {
			return ("null");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdNull);
		}

		public override bool IsConstant { get { return (true); } }
	}
}
