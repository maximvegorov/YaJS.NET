using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

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

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			// Надо учесть возможность побочных эффектов вызова выражений
			foreach (var item in _items)
				item.CompileBy(compiler, isLast);
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdInteger, _items.Count);
			compiler.Emitter.Emit(OpCode.MakeArray);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
