using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class VoidOperator : UnaryOperator {
		public VoidOperator(Expression operand)
			: base(operand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append("void(").Append(Operand.ToString()).Append(')');
			return (result.ToString());
		}
	}
}
