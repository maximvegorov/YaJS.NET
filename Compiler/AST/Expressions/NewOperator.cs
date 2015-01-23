using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class NewOperator : Expression {
		internal NewOperator(Expression constructor, List<Expression> arguments) : base(ExpressionType.New) {
			Contract.Requires(constructor != null && constructor.CanBeConstructor);
			Contract.Requires(arguments != null);
			Constructor = constructor;
			Arguments = arguments;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append("new ")
				.Append(Constructor)
				.Append('(');
			if (Arguments.Count > 0) {
				foreach (var argument in Arguments) {
					result.Append(argument)
						.Append(',');
				}
				result.Length -= 1;
			}
			result.Append(')');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			Constructor.CompileBy(compiler, false);
			foreach (var argument in Arguments)
				argument.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdInteger, Arguments.Count);
			compiler.Emitter.Emit(OpCode.NewObj);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Expression Constructor { get; private set; }
		public List<Expression> Arguments { get; private set; }
	}
}
