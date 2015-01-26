using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class NeqOperator : BinaryOperator {
		public NeqOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.Neq, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" != ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileEqualityBy(compiler, OpCode.StrictNeq, OpCode.ConvNeq, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
