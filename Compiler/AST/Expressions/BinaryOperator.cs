using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех бинарных операторов
	/// </summary>
	internal abstract class BinaryOperator : Expression {
		protected BinaryOperator(ExpressionType type, Expression leftOperand, Expression rightOperand)
			: base(type) {
			Contract.Requires(leftOperand != null);
			Contract.Requires(rightOperand != null);
			LeftOperand = leftOperand;
			RightOperand = rightOperand;
		}

		public override bool IsConstant {
			get { return (LeftOperand.IsConstant && RightOperand.IsConstant); }
		}

		public Expression LeftOperand { get; private set; }
		public Expression RightOperand { get; private set; }
	}
}
