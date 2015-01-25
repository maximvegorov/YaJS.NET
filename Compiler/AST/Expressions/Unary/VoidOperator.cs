using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class VoidOperator : UnaryOperator {
		public VoidOperator(Expression operand)
			: base(ExpressionType.Void, operand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append("void(").Append(Operand).Append(')');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Operand.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.Pop);
			if (!isLastOperator)
				compiler.Emitter.Emit(OpCode.LdUndefined);
		}
	}
}
