using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

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

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			OpCode callOpCode;
			if (_function.Type != ExpressionType.Member) {
				_function.CompileBy(compiler, false);
				callOpCode = OpCode.Call;
			}
			else {
				var memberOperator = _function as MemberOperator;
				Contract.Assert(memberOperator != null);
				memberOperator.BaseValue.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.Dup);
				memberOperator.Member.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.LdMember);
				callOpCode = OpCode.CallMember;
			}
			foreach (var argument in _arguments)
				argument.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdInteger, _arguments.Count);
			compiler.Emitter.Emit(callOpCode, !isLast);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
