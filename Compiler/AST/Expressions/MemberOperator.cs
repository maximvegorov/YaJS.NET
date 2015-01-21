using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class MemberOperator : Expression {
		private readonly Expression _baseValue;
		private readonly Expression _member;

		public MemberOperator(Expression baseValue, Expression member)
			: base(ExpressionType.Member) {
			Contract.Requires(baseValue != null && baseValue.CanHaveMembers);
			Contract.Requires(member != null);
			_baseValue = baseValue;
			_member = member;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(_baseValue)
				.Append('[')
				.Append(_member)
				.Append(']');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			_baseValue.CompileBy(compiler, false);
			_member.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdMember);
		}

		public override bool IsReference { get { return (_baseValue.CanHaveMutableMembers); } }
		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeDeleted { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Expression BaseValue { get { return (_baseValue); } }
		public Expression Member { get { return (_member); } }
	}
}
