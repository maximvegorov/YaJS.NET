using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class DeleteOperator : UnaryOperator {
		public DeleteOperator(Expression operand)
			: base(operand) {
			Contract.Requires(operand.IsReference);
		}

		public override string ToString() {
			return ("delete " + Operand.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
	}
}
