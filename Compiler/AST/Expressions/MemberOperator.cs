using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class MemberOperator : Expression {
		public MemberOperator(Expression baseValue, Expression member) {
			Contract.Requires(baseValue != null && baseValue.CanHaveMembers);
			Contract.Requires(member != null);
			BaseValue = baseValue;
			Member = member;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(BaseValue.ToString())
				.Append('[')
				.Append(Member.ToString())
				.Append(']');
			return (result.ToString());
		}

		public override bool IsReference { get { return (BaseValue.CanHaveMutableMembers); } }
		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeDeleted { get { return (true); } }

		public Expression BaseValue { get; private set; }
		public new Expression Member { get; private set; }
	}
}
