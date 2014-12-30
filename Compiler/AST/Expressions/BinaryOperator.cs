﻿using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех бинарных операторов
	/// </summary>
	internal abstract class BinaryOperator : Expression {
		public BinaryOperator(Expression leftOperand, Expression rightOperand) {
			Contract.Requires(leftOperand != null);
			Contract.Requires(rightOperand != null);
			LeftOperand = leftOperand;
			RightOperand = rightOperand;
		}

		public Expression LeftOperand { get; private set; }
		public Expression RightOperand { get; private set; }
	}
}