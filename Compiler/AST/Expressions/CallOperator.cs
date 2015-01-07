using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class CallOperator : Expression {
		public CallOperator(Expression function, List<Expression> arguments) {
			Contract.Requires(function != null && function.CanBeFunction);
			Contract.Requires(arguments != null);
			Function = function;
			Arguments = arguments;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(Function.ToString())
				.Append('(');
			if (Arguments.Count > 0) {
				foreach (var argument in Arguments) {
					result.Append(argument.ToString())
						.Append(',');
				}
				result.Length -= 1;
			}
			result.Append(')');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }

		public Expression Function { get; private set; }
		public new List<Expression> Arguments { get; private set; }
	}
}
