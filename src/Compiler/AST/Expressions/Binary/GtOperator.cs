﻿using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class GtOperator : BinaryOperator {
		public GtOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Gt, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" > ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileBy(compiler, OpCode.Gt, true, true, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
