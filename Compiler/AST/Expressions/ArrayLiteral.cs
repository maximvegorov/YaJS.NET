using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class ArrayLiteral : Expression {
		internal ArrayLiteral(List<Expression> items)
			: base(ExpressionType.ArrayLiteral) {
			Contract.Requires(items != null);
			Items = items;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			if (Items.Count > 0) {
				foreach (var item in Items)
					result.Append(item).Append(',');
				result.Length -= 1;
			}
			result.Append(']');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (Items.Count == 0) {
				if (isLastOperator)
					return;
				compiler.Emitter.Emit(OpCode.MakeEmptyArray);
			}
			else {
				foreach (var item in Items)
					item.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.LdInteger, Items.Count);
				compiler.Emitter.Emit(OpCode.MakeArray);
				if (isLastOperator)
					compiler.Emitter.Emit(OpCode.Pop);
			}
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public List<Expression> Items { get; private set; }
	}
}
