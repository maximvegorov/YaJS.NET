using System.Globalization;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class IntegerLiteral : Expression {
		private readonly int _value;

		public IntegerLiteral(int value) : base(ExpressionType.IntegerLiteral) {
			_value = value;
		}

		public override string ToString() {
			return (_value.ToString(CultureInfo.InvariantCulture));
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
		public override bool CanBeUsedInCaseClause { get { return (true); } }
	}
}
