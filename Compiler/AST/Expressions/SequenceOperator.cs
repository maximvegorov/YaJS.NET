using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

		public List<Expression> Operands { get; private set; }
	}
}
