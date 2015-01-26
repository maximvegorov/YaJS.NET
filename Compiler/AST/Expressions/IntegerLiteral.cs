using System.Globalization;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class IntegerLiteral : Expression {
		internal IntegerLiteral(int value)
			: base(ExpressionType.IntegerLiteral) {
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString(CultureInfo.InvariantCulture));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdInteger, Value);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
		public override bool CanBeUsedInCaseClause { get { return (true); } }
		public int Value { get; private set; }
	}
}
