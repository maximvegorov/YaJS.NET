using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех бинарных операторов
	/// </summary>
	internal abstract class BinaryOperator : Expression {
		protected BinaryOperator(Expression leftOperand, Expression rightOperand) {
			Contract.Requires(leftOperand != null);
			Contract.Requires(rightOperand != null);
			LeftOperand = leftOperand;
			RightOperand = rightOperand;
		}

		public override bool IsConstant { get { return (LeftOperand.IsConstant && RightOperand.IsConstant); } }

		protected Expression LeftOperand { get; private set; }
		protected Expression RightOperand { get; private set; }
	}
}
