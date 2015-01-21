using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ConditionalOperator : Expression {
		public ConditionalOperator(Expression condition, Expression trueOperand, Expression falseOperand)
			: base(ExpressionType.Conditional) {
			Contract.Requires(condition != null);
			Contract.Requires(trueOperand != null);
			Contract.Requires(falseOperand != null);
			Condition = condition;
			TrueOperand = trueOperand;
			FalseOperand = falseOperand;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(Condition)
				.Append('?')
				.Append(TrueOperand)
				.Append(':')
				.Append(FalseOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			var endLabel = compiler.Emitter.DefineLabel();
			var falseLabel = compiler.Emitter.DefineLabel();
			Condition.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.GotoIfFalse, falseLabel);
			TrueOperand.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.Goto, endLabel);
			compiler.Emitter.MarkLabel(falseLabel);
			FalseOperand.CompileBy(compiler, false);
			compiler.Emitter.MarkLabel(endLabel);
		}

		public override bool CanHaveMembers { get { return (TrueOperand.CanHaveMembers || FalseOperand.CanHaveMembers); } }

		public override bool CanHaveMutableMembers {
			get { return (TrueOperand.CanHaveMutableMembers || FalseOperand.CanHaveMutableMembers); }
		}

		public override bool CanBeConstructor {
			get { return (TrueOperand.CanBeConstructor || FalseOperand.CanBeConstructor); }
		}

		public override bool CanBeFunction { get { return (TrueOperand.CanBeFunction || FalseOperand.CanBeFunction); } }
		public override bool CanBeObject { get { return (TrueOperand.CanBeObject || FalseOperand.CanBeObject); } }

		public override bool IsConstant {
			get { return (Condition.IsConstant && TrueOperand.IsConstant && FalseOperand.IsConstant); }
		}

		public Expression Condition { get; private set; }
		public Expression TrueOperand { get; private set; }
		public Expression FalseOperand { get; private set; }
	}
}
