using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех унарных операторов
	/// </summary>
	internal abstract class UnaryOperator : Expression {
		public UnaryOperator(Expression operand) {
			Contract.Requires(operand != null);
			Operand = operand;
		}

		public override bool IsConstant { get { return (Operand.IsConstant); } }

		public Expression Operand { get; private set; }
	}
}
