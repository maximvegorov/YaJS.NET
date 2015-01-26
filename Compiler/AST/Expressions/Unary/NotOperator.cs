using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NotOperator : UnaryOperator {
		public NotOperator(Expression operand)
			: base(ExpressionType.Not, operand) {
		}

		public override string ToString() {
			return ("!" + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Operand.CompileBy(compiler, false);
			compiler.Emitter.Emit(isLastOperator ? OpCode.Pop : OpCode.Not);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
