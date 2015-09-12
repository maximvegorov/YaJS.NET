using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PosOperator : UnaryOperator {
		public PosOperator(Expression operand)
			: base(ExpressionType.Pos, operand) {
		}

		public override string ToString() {
			return ("+" + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Operand.CompileBy(compiler, false);
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
			else {
				if (Operand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(OpCode.Pos);
			}
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
