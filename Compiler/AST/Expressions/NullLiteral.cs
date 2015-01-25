using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class NullLiteral : Expression {
		internal NullLiteral()
			: base(ExpressionType.NullLiteral) {
		}

		public override string ToString() {
			return ("null");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdNull);
		}

		public override bool IsConstant {
			get { return (true); }
		}
	}
}
