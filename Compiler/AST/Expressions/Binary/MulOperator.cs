using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class MulOperator : BinaryOperator {
		public MulOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Mul, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" * ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileBy(compiler, OpCode.Mul, true, true, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
