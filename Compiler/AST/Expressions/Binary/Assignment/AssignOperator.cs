using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal abstract class AssignOperator : BinaryOperator {
		protected AssignOperator(ExpressionType type, Expression leftOperand, Expression rightOperand)
			: base(type, leftOperand, rightOperand) {
			Contract.Requires(leftOperand.IsReference);
		}

		protected void CompileAssignBy(FunctionCompiler compiler, OpCode op, bool isLastOperator) {
			if (LeftOperand.Type == ExpressionType.Ident) {
				var leftOperand = (Identifier)LeftOperand;
				compiler.Emitter.Emit(OpCode.LdLocal, leftOperand.Value);
				compiler.Emitter.Emit(OpCode.CastToPrimitive);
				RightOperand.CompileBy(compiler, false);
				if (RightOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(op);
				if (!isLastOperator)
					compiler.Emitter.Emit(OpCode.Dup);
				compiler.Emitter.Emit(OpCode.StLocal, leftOperand.Value);
			}
			else {
				Contract.Assert(LeftOperand.Type == ExpressionType.Member);
				var leftOperand = (MemberOperator)LeftOperand;
				leftOperand.BaseValue.CompileBy(compiler, false);
				leftOperand.CompilePropertyBy(compiler);
				compiler.Emitter.Emit(OpCode.MakeRef);
				compiler.Emitter.Emit(OpCode.Dup);
				compiler.Emitter.Emit(OpCode.LdMemberByRef);
				compiler.Emitter.Emit(OpCode.CastToPrimitive);
				RightOperand.CompileBy(compiler, false);
				if (RightOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(op);
				compiler.Emitter.Emit(isLastOperator ? OpCode.StMemberByRef : OpCode.StMemberByRefDup);
			}
		}

		public override bool CanHaveMembers { get { return (RightOperand.CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (RightOperand.CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (RightOperand.CanBeConstructor); } }
		public override bool CanBeFunction { get { return (RightOperand.CanBeFunction); } }
		public override bool CanBeObject { get { return (RightOperand.CanBeObject); } }
		public override bool IsConstant { get { return (false); } }
	}
}
