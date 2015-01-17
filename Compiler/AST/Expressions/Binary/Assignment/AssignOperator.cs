﻿using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal abstract class AssignOperator : BinaryOperator {
		protected AssignOperator(ExpressionType type, Expression leftOperand, Expression rightOperand)
			: base(type, leftOperand, rightOperand) {
			Contract.Requires(leftOperand.IsReference);
		}

		public override bool CanHaveMembers { get { return (RightOperand.CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (RightOperand.CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (RightOperand.CanBeConstructor); } }
		public override bool CanBeFunction { get { return (RightOperand.CanBeFunction); } }
		public override bool IsConstant { get { return (false); } }
	}
}
