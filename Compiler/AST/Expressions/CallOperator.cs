using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class CallOperator : Expression {
		private readonly Expression _function;
		private readonly List<Expression> _arguments;

		public CallOperator(Expression function, List<Expression> arguments)
			: base(ExpressionType.Call) {
			Contract.Requires(function != null && function.CanBeFunction);
			Contract.Requires(arguments != null);
			_function = function;
			_arguments = arguments;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(_function)
				.Append('(');
			if (_arguments.Count > 0) {
				foreach (var argument in _arguments) {
					result.Append(argument)
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
		public override bool CanBeObject { get { return (true); } }
	}
}
