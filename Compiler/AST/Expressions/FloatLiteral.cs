using System.Globalization;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class FloatLiteral : Expression {
		internal FloatLiteral(double value)
			: base(ExpressionType.FloatLiteral) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		public override bool Equals(object obj) {
			var other = obj as FloatLiteral;
			return (other != null && Value == other.Value);
		}

		public override int GetHashCode() {
			return (GetHashCode(Type.GetHashCode(), Value.GetHashCode()));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdFloat, Value);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool IsConstant {
			get { return (true); }
		}

		public double Value { get; private set; }
	}
}
