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

		public override bool Equals(object obj) {
			var other = obj as BinaryOperator;
			return (other != null && Type == other.Type && LeftOperand.Equals(other.LeftOperand) &&
				RightOperand.Equals(other.RightOperand));
		}

		public override int GetHashCode() {
			return (GetHashCode(GetHashCode(Type.GetHashCode(), LeftOperand.GetHashCode()), RightOperand.GetHashCode()));
		}

		public override bool IsConstant {
			get { return (LeftOperand.IsConstant && RightOperand.IsConstant); }
		}

		public Expression LeftOperand { get; private set; }
		public Expression RightOperand { get; private set; }
	}
}
