using System.Diagnostics.Contracts;
using YaJS.Runtime;

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
			CompileIncDecBy(compiler, OpCode.Inc, false, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
