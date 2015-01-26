using System.Diagnostics.Contracts;
using YaJS.Runtime;

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

		internal void CompileIncDecBy(FunctionCompiler compiler, bool isInc, bool isPostfix, bool isLastOperator) {
			if (Operand.Type == ExpressionType.Ident) {
				var operand = (Identifier)Operand;
				compiler.Emitter.Emit(OpCode.LdLocal, operand.Value);
				compiler.Emitter.Emit(OpCode.CastToPrimitive);
				var op = isInc ? OpCode.Inc : OpCode.Dec;
				if (isPostfix) {
					if (!isLastOperator)
						compiler.Emitter.Emit(OpCode.Dup);
					compiler.Emitter.Emit(op);
				}
				else {
					compiler.Emitter.Emit(op);
					if (!isLastOperator)
						compiler.Emitter.Emit(OpCode.Dup);
				}
				compiler.Emitter.Emit(OpCode.StLocal, operand.Value);
			}
			else {
				Contract.Assert(Operand.Type == ExpressionType.Member);
				var operand = (MemberOperator)Operand;
				operand.BaseValue.CompileBy(compiler, false);
				operand.CompilePropertyBy(compiler);
				compiler.Emitter.Emit(OpCode.MakeRef);
				compiler.Emitter.Emit(OpCode.Dup);
				compiler.Emitter.Emit(OpCode.LdMemberByRef);
				compiler.Emitter.Emit(OpCode.CastToPrimitive);
				compiler.Emitter.Emit(isInc ? OpCode.IncMemberByRef : OpCode.DecMemberByRef, isPostfix);
				if (isLastOperator)
					compiler.Emitter.Emit(OpCode.Dup);
			}
		}

		public override bool IsConstant {
			get { return (Operand.IsConstant); }
		}

		public Expression Operand { get; private set; }
	}
}
