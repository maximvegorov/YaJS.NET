using System.Globalization;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class IntegerLiteral : Expression {
		public IntegerLiteral(int value)
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

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool IsConstant {
			get { return (true); }
		}

		public override bool CanBeUsedInCaseClause {
			get { return (true); }
		}

		public int Value { get; private set; }
	}
}
