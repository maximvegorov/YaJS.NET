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
