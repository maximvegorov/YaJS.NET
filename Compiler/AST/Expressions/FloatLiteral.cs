using System.Globalization;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class FloatLiteral : Expression {
		public FloatLiteral(double value) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }

		public double Value { get; private set; }
	}
}
