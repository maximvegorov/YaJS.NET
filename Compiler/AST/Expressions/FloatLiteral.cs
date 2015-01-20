using System.Globalization;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class FloatLiteral : Expression {
		private readonly double _value;

		public FloatLiteral(double value)
			: base(ExpressionType.FloatLiteral) {
			_value = value;
		}

		public override string ToString() {
			return (_value.ToString(CultureInfo.InvariantCulture));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdFloat, _value);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
	}
}
