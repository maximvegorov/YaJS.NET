using System.Globalization;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class FloatLiteral : Expression {
		public FloatLiteral(double value)
			: base(ExpressionType.FloatLiteral) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
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
