using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class SimpleAssignOperator : AssignOperator {
		public SimpleAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.SimpleAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" = ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (LeftOperand.Type == ExpressionType.Ident) {
				var leftOperand = (Identifier)LeftOperand;
				RightOperand.CompileBy(compiler, false);
				if (!isLastOperator)
					compiler.Emitter.Emit(OpCode.Dup);
				compiler.Emitter.Emit(OpCode.StLocal, leftOperand.Value);
			}
			else {
				Contract.Assert(LeftOperand.Type == ExpressionType.Member);
				var leftOperand = (MemberOperator)LeftOperand;
				leftOperand.BaseValue.CompileBy(compiler, false);
				leftOperand.CompilePropertyBy(compiler);
				RightOperand.CompileBy(compiler, false);
				compiler.Emitter.Emit(isLastOperator ? OpCode.StMember : OpCode.StMemberDup);
			}
		}

		public override bool CanBeObject { get { return (RightOperand.CanBeObject); } }
	}
}
