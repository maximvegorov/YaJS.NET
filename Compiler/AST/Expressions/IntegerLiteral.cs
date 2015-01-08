using System.Globalization;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class IntegerLiteral : Expression {
		public IntegerLiteral(int value) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
		public override bool CanBeUsedInCaseClause { get { return (true); } }

		public int Value { get; private set; }
	}
}
