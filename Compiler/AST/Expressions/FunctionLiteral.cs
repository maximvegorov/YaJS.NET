using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class FunctionLiteral : Expression {
		public FunctionLiteral(Function function) {
			Contract.Requires(function != null);
			Function = function;
		}

		public override string ToString() {
			return (Function.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }

		public new Function Function { get; private set; }
	}
}
