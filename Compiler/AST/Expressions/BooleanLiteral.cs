using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BooleanLiteral : Expression {
		private readonly bool _value;

		public BooleanLiteral(bool value)
			: base(ExpressionType.BooleanLiteral) {
			_value = value;
		}

		public override string ToString() {
			return (_value ? "true" : "false");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdBoolean, _value);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
	}
}
