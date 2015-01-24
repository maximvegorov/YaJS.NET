using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class MemberOperator : Expression {
		internal MemberOperator(Expression baseValue, Expression memberName)
			: base(ExpressionType.Member) {
			Contract.Requires(baseValue != null && baseValue.CanHaveMembers);
			Contract.Requires(memberName != null);
			BaseValue = baseValue;
			MemberName = memberName;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(BaseValue)
				.Append('[')
				.Append(MemberName)
				.Append(']');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			BaseValue.CompileBy(compiler, false);
			MemberName.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdMember);
		}

		public override bool IsReference { get { return (BaseValue.CanHaveMutableMembers); } }
		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeDeleted { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Expression BaseValue { get; private set; }
		public Expression MemberName { get; private set; }
	}
}
