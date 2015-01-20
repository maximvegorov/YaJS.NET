using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal class GroupingOperator : Expression {
		private readonly Expression _operand;

		public GroupingOperator(Expression operand)
			: base(ExpressionType.Grouping) {
			Contract.Requires(operand != null);
			_operand = operand;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('(').Append(_operand).Append(')');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			_operand.CompileBy(compiler, isLast);
		}

		public override bool CanHaveMembers { get { return (_operand.CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (_operand.CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (_operand.CanBeConstructor); } }
		public override bool CanBeFunction { get { return (_operand.CanBeFunction); } }
		public override bool CanBeDeleted { get { return (_operand.CanBeDeleted); } }
		public override bool CanBeObject { get { return (_operand.CanBeObject); } }
		public override bool IsConstant { get { return (_operand.IsConstant); } }
	}
}
