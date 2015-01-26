using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BooleanLiteral : Expression {
		public BooleanLiteral(bool value)
			: base(ExpressionType.BooleanLiteral) {
			Value = value;
		}

		public override string ToString() {
			return (Value ? "true" : "false");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdBoolean, Value);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool IsConstant {
			get { return (true); }
		}

		public bool Value { get; private set; }
	}
}
