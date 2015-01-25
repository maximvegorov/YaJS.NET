using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PostfixDecOperator : UnaryOperator {
		public PostfixDecOperator(Expression operand)
			: base(ExpressionType.PostfixDec, operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return (Operand + "--");
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			compiler.CompileIncDecExpression(Operand, false, true, isLastOperator);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
