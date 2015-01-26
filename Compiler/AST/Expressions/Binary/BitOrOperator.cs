using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class BitOrOperator : BinaryOperator {
		public BitOrOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.BitOr, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" | ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileBy(compiler, OpCode.BitOr, true, true, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
