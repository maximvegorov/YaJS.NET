using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class InstanceOfOperator : BinaryOperator {
		public InstanceOfOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.InstanceOf, leftOperand, rightOperand) {
			Contract.Requires(rightOperand.CanBeConstructor);
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" instanceof ").Append(RightOperand);
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			CompileBy(compiler, OpCode.InstanceOf, true, false, isLastOperator);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}
	}
}
