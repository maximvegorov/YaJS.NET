using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ArrayLiteral : Expression {
		private readonly List<Expression> _items;

		public ArrayLiteral(List<Expression> items)
			: base(ExpressionType.ArrayLiteral) {
			Contract.Requires(items != null);
			_items = items;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			if (_items.Count > 0) {
				foreach (var item in _items) {
					result.Append(item)
						.Append(',');
				}
				result.Length -= 1;
			}
			result.Append(']');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
