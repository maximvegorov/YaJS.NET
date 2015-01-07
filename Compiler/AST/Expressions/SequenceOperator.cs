using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class SequenceOperator : Expression {
		public SequenceOperator(List<Expression> operands) {
			Contract.Requires(operands != null && operands.Count > 1);
			Operands = operands;
		}

		public override string ToString() {
			var result = new StringBuilder();
			foreach (var operand in Operands) {
				result.Append('(').Append(operand.ToString()).Append('(')
					.Append(',');
			}
			result.Length -= 1;
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (Operands[Operands.Count - 1].CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (Operands[Operands.Count - 1].CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (Operands[Operands.Count - 1].CanBeConstructor); } }
		public override bool CanBeFunction { get { return (Operands[Operands.Count - 1].CanBeFunction); } }
		public override bool CanBeDeleted { get { return (Operands[Operands.Count - 1].CanBeDeleted); } }
		public override bool IsConstant { get { return (Operands.All(o => o.IsConstant)); } }

		public List<Expression> Operands { get; private set; }
	}
}
