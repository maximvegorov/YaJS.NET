using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ShrUAssignOperator : AssignOperator {
		public ShrUAssignOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.ShrUAssign, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" >>>= ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileAssignBy(compiler, OpCode.ShrU, isLastOperator);
		}
	}
}
