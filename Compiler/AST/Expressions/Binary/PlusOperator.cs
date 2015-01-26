using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class PlusOperator : BinaryOperator {
		public PlusOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Or, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" + ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileBy(compiler, OpCode.Plus, true, true, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
