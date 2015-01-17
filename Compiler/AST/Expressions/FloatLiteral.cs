using System.Globalization;

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

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
	}
}
