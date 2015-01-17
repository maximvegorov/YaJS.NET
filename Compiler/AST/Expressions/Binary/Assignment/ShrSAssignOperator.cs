﻿using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ShrSAssignOperator : AssignOperator {
		public ShrSAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.ShrSAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" >>= ").Append(RightOperand);
			return (result.ToString());
		}
	}
}
