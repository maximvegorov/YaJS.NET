using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class SequenceOperator : Expression {
		private readonly List<Expression> _operands;

		public SequenceOperator(List<Expression> operands) : base(ExpressionType.Sequence) {
			Contract.Requires(operands != null && operands.Count > 1);
			_operands = operands;
		}

		public override string ToString() {
			var result = new StringBuilder();
			foreach (var operand in _operands) {
				result.Append('(').Append(operand).Append('(')
					.Append(',');
			}
			result.Length -= 1;
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (_operands[_operands.Count - 1].CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (_operands[_operands.Count - 1].CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (_operands[_operands.Count - 1].CanBeConstructor); } }
		public override bool CanBeFunction { get { return (_operands[_operands.Count - 1].CanBeFunction); } }
		public override bool CanBeDeleted { get { return (_operands[_operands.Count - 1].CanBeDeleted); } }
		public override bool CanBeObject { get { return (_operands[_operands.Count - 1].CanBeObject); } }
		public override bool IsConstant { get { return (_operands.All(o => o.IsConstant)); } }
	}
}
