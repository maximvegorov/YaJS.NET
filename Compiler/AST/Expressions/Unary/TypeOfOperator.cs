using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class TypeOfOperator : UnaryOperator {
		public TypeOfOperator(Expression operand)
			: base(ExpressionType.TypeOf, operand) {
		}

		public override string ToString() {
			return ("typeof " + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Operand.CompileBy(compiler, false);
			compiler.Emitter.Emit(isLastOperator ? OpCode.Pop : OpCode.TypeOf);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
