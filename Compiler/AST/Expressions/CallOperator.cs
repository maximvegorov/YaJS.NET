using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class CallOperator : Expression {
		internal CallOperator(Expression callFunction, List<Expression> argumentList)
			: base(ExpressionType.Call) {
			Contract.Requires(callFunction != null && callFunction.CanBeFunction);
			Contract.Requires(argumentList != null);
			CallFunction = callFunction;
			ArgumentList = argumentList;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(CallFunction).Append('(');
			if (ArgumentList.Count > 0) {
				foreach (var argument in ArgumentList)
					result.Append(argument).Append(',');
				result.Length -= 1;
			}
			result.Append(')');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			OpCode callOpCode;
			if (CallFunction.Type != ExpressionType.Member) {
				CallFunction.CompileBy(compiler, false);
				callOpCode = OpCode.Call;
			}
			else {
				var memberOperator = CallFunction as MemberOperator;
				Contract.Assert(memberOperator != null);
				memberOperator.BaseValue.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.Dup);
				memberOperator.Property.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.LdMember);
				callOpCode = OpCode.CallMember;
			}
			foreach (var argument in ArgumentList)
				argument.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdInteger, ArgumentList.Count);
			compiler.Emitter.Emit(callOpCode, !isLast);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool CanHaveMutableMembers {
			get { return (true); }
		}

		public override bool CanBeConstructor {
			get { return (true); }
		}

		public override bool CanBeFunction {
			get { return (true); }
		}

		public override bool CanBeObject {
			get { return (true); }
		}

		public Expression CallFunction { get; private set; }
		public List<Expression> ArgumentList { get; private set; }
	}
}
