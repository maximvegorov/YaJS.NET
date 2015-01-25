using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DecOperator : UnaryOperator {
		public DecOperator(Expression operand)
			: base(ExpressionType.Dec, operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return ("--" + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			compiler.CompileIncDecExpression(Operand, false, false, isLastOperator);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
