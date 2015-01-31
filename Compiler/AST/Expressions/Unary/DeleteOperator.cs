using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DeleteOperator : UnaryOperator {
		public DeleteOperator(Expression operand)
			: base(ExpressionType.Delete, operand) {
			Contract.Requires(operand.CanBeDeleted);
		}

		public override string ToString() {
			return ("delete " + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			var target = Operand;
			while (target.Type == ExpressionType.Grouping)
				target = ((GroupingOperator)target).Operand;
			Contract.Assert(target.Type == ExpressionType.Member);
			var operand = (MemberOperator)target;
			operand.BaseValue.CompileBy(compiler, false);
			operand.CompilePropertyBy(compiler);
			compiler.Emitter.Emit(OpCode.DelMember);
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
		}
	}
}
