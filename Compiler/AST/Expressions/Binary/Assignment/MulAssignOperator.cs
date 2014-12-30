﻿using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class MulAssignOperator : AssignOperator {
		public MulAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand.ToString()).Append(" *= ").Append(RightOperand.ToString());
			return (result.ToString());
		}
	}
}