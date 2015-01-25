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

		public override bool Equals(object obj) {
			var other = obj as BooleanLiteral;
			return (other != null && Value == other.Value);
		}

		public override int GetHashCode() {
			return (GetHashCode(Type.GetHashCode(), Value.GetHashCode()));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
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
