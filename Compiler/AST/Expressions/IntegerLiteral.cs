using System.Globalization;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class IntegerLiteral : Expression {
		private readonly int _value;

		public IntegerLiteral(int value) : base(ExpressionType.IntegerLiteral) {
			_value = value;
		}

		public override string ToString() {
			return (_value.ToString(CultureInfo.InvariantCulture));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdInteger, _value);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool IsConstant { get { return (true); } }
		public override bool CanBeUsedInCaseClause { get { return (true); } }
	}
}
