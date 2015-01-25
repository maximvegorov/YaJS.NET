using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class InOperator : BinaryOperator {
		public InOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.In, leftOperand, rightOperand) {
			Contract.Requires(rightOperand.CanHaveMembers);
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" in ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileBy(compiler, OpCode.IsMember, true, false, isLastOperator);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
