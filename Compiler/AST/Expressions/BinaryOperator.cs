using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	/// <summary>
	/// Базовый класс для всех бинарных операторов
	/// </summary>
	public abstract class BinaryOperator : Expression {
		protected BinaryOperator(ExpressionType type, Expression leftOperand, Expression rightOperand)
			: base(type) {
			Contract.Requires(leftOperand != null);
			Contract.Requires(rightOperand != null);
			LeftOperand = leftOperand;
			RightOperand = rightOperand;
		}

		internal void CompileBy(
			FunctionCompiler compiler,
			OpCode op,
			bool isLeftAssociative,
			bool needCastToPrimitive,
			bool isLastOperator) {
			if (isLeftAssociative) {
				LeftOperand.CompileBy(compiler, false);
				if (needCastToPrimitive && LeftOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				RightOperand.CompileBy(compiler, false);
				if (needCastToPrimitive && RightOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
			}
			else {
				RightOperand.CompileBy(compiler, false);
				if (needCastToPrimitive && RightOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				LeftOperand.CompileBy(compiler, false);
				if (needCastToPrimitive && LeftOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
			}
			compiler.Emitter.Emit(op);
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
		}

		internal void CompileEqualityBy(FunctionCompiler compiler, OpCode strictOp, OpCode convOp, bool isLastOperator) {
			if (!LeftOperand.CanBeObject && !RightOperand.CanBeObject) {
				LeftOperand.CompileBy(compiler, false);
				RightOperand.CompileBy(compiler, false);
				compiler.Emitter.Emit(strictOp);
			}
			else {
				var endLabel = compiler.Emitter.DefineLabel();
				var falseLabel = compiler.Emitter.DefineLabel();
				LeftOperand.CompileBy(compiler, false);
				RightOperand.CompileBy(compiler, false);
				compiler.Emitter.Emit(OpCode.Dup2);
				compiler.Emitter.Emit(strictOp);
				compiler.Emitter.Emit(OpCode.GotoIfFalse, falseLabel);
				compiler.Emitter.Emit(OpCode.Pop2);
				compiler.Emitter.Emit(OpCode.LdBoolean, true);
				compiler.Emitter.Emit(OpCode.Goto, endLabel);
				compiler.Emitter.MarkLabel(falseLabel);
				// В вершине стека лежит правый операнд
				if (RightOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(OpCode.Swap);
				if (LeftOperand.CanBeObject)
					compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(convOp);
				compiler.Emitter.MarkLabel(endLabel);
			}
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
		}

		public override bool IsConstant { get { return (LeftOperand.IsConstant && RightOperand.IsConstant); } }
		public Expression LeftOperand { get; private set; }
		public Expression RightOperand { get; private set; }
	}
}
