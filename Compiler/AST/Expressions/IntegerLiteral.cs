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

		public override bool Equals(object obj) {
			var other = obj as IntegerLiteral;
			return (other != null && Value == other.Value);
		}

		public override int GetHashCode() {
			return (GetHashCode(Type.GetHashCode(), Value.GetHashCode()));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
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
