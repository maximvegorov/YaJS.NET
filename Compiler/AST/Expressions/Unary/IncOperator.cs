using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class IncOperator : UnaryOperator {
		public IncOperator(Expression operand)
			: base(ExpressionType.Inc, operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return ("++" + Operand);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			compiler.CompileIncDecExpression(Operand, true, false, isLastOperator);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
