using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal abstract class AssignOperator : BinaryOperator {
		public AssignOperator(Expression leftOperand, Expression rightOperand)
			: base(leftOperand, rightOperand) {
			Contract.Requires(leftOperand.IsReference);
		}
	}
}
