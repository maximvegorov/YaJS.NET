using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BitNotOperator : UnaryOperator {
		public BitNotOperator(Expression operand)
			: base(ExpressionType.BitNot, operand) {
		}

		public override string ToString() {
			return ('~' + Operand.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Operand.CompileBy(compiler, false);
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
			else {
				if (Operand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(OpCode.BitNot);
			}
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
