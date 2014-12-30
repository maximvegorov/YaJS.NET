using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ArrayLiteral : Expression {
		public ArrayLiteral(List<Expression> items) {
			Contract.Requires(items != null);
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			if (Items.Count > 0) {
				foreach (var item in Items) {
					result.Append(item.ToString())
						.Append(',');
				}
				result.Length -= 1;
			}
			result.Append(']');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }

		public List<Expression> Items { get; private set; }
	}
}
