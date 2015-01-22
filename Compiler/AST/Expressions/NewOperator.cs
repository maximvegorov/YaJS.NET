using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NewOperator : Expression {
		private readonly Expression _constructor;
		private readonly List<Expression> _arguments;

		public NewOperator(Expression constructor, List<Expression> arguments) : base(ExpressionType.New) {
			Contract.Requires(constructor != null && constructor.CanBeConstructor);
			Contract.Requires(arguments != null);
			_constructor = constructor;
			_arguments = arguments;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append("new ")
				.Append(_constructor)
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

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			_constructor.CompileBy(compiler, false);
			foreach (var argument in _arguments)
				argument.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdInteger, _arguments.Count);
			compiler.Emitter.Emit(OpCode.NewObj);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
