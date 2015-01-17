using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DeleteOperator : UnaryOperator {
		public DeleteOperator(Expression operand)
			: base(ExpressionType.Delete, operand) {
			Contract.Requires(operand.CanBeDeleted);
		}

		public override string ToString() {
			return ("delete " + Operand);
		}
	}
}
