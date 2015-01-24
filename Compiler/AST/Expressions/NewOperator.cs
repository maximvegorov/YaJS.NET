using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class NewOperator : Expression {
		internal NewOperator(Expression constructor, List<Expression> argumentList) : base(ExpressionType.New) {
			Contract.Requires(constructor != null && constructor.CanBeConstructor);
			Contract.Requires(argumentList != null);
			Constructor = constructor;
			ArgumentList = argumentList;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append("new ")
				.Append(Constructor)
				.Append('(');
			if (ArgumentList.Count > 0) {
				foreach (var argument in ArgumentList) {
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
			foreach (var argument in ArgumentList)
				argument.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdInteger, ArgumentList.Count);
			compiler.Emitter.Emit(OpCode.NewObj);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Expression Constructor { get; private set; }
		public List<Expression> ArgumentList { get; private set; }
	}
}
