using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class BooleanLiteral : Expression {
		internal BooleanLiteral(bool value)
			: base(ExpressionType.BooleanLiteral) {
			Value = value;
		}

		public override string ToString() {
			return (Value ? "true" : "false");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdBoolean, Value);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
		public bool Value { get; private set; }
	}
}
