using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NegOperator : UnaryOperator {
		public NegOperator(Expression operand)
			: base(ExpressionType.Neg, operand) {
		}

		public override string ToString() {
			return ("-" + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Operand.CompileBy(compiler, false);
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
			else {
				if (Operand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(OpCode.Neg);
			}
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
