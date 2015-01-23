using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class CallOperator : Expression {
		internal CallOperator(Expression function, List<Expression> arguments)
			: base(ExpressionType.Call) {
			Contract.Requires(function != null && function.CanBeFunction);
			Contract.Requires(arguments != null);
			Function = function;
			Arguments = arguments;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(Function)
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
			OpCode callOpCode;
			if (Function.Type != ExpressionType.Member) {
				Function.CompileBy(compiler, false);
				callOpCode = OpCode.Call;
			}
			else {
				var memberOperator = Function as MemberOperator;
				Contract.Assert(memberOperator != null);
				memberOperator.BaseValue.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.Dup);
				memberOperator.Member.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.LdMember);
				callOpCode = OpCode.CallMember;
			}
			foreach (var argument in Arguments)
				argument.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdInteger, Arguments.Count);
			compiler.Emitter.Emit(callOpCode, !isLast);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Expression Function { get; private set; }
		public List<Expression> Arguments { get; private set; }
	}
}
