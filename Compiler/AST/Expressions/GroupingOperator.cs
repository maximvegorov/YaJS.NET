using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal class GroupingOperator : Expression {
		public GroupingOperator(Expression operand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('(').Append(Operand.ToString()).Append(')');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (Operand.CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (Operand.CanBeConstructor); } }
		public override bool CanBeFunction { get { return (Operand.CanBeFunction); } }
		public override bool CanBeDeleted { get { return (Operand.CanBeDeleted); } }

		public Expression Operand { get; private set; }
	}
}
