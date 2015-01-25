using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех унарных операторов
	/// </summary>
	public abstract class UnaryOperator : Expression {
		protected UnaryOperator(ExpressionType type, Expression operand)
			: base(type) {
			Contract.Requires(operand != null);
			Operand = operand;
		}

		public override bool IsConstant {
			get { return (Operand.IsConstant); }
		}

		public Expression Operand { get; private set; }
	}
}
