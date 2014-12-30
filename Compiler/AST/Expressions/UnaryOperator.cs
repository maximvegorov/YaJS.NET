using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех унарных операторов
	/// </summary>
	internal abstract class UnaryOperator {
		public UnaryOperator(Expression operand) {
			Contract.Requires(operand != null);
			Operand = operand;
		}

		public Expression Operand { get; private set; }
	}
}
