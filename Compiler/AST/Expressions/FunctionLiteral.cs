using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class FunctionLiteral : Expression {
		internal FunctionLiteral(Function value)
			: base(ExpressionType.FunctionLiteral) {
			Contract.Requires(value != null);
			Value = value;
		}

		public override string ToString() {
			return (Value.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdLocalFunc, Value.Index);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool CanHaveMutableMembers {
			get { return (true); }
		}

		public override bool CanBeConstructor {
			get { return (true); }
		}

		public override bool CanBeFunction {
			get { return (true); }
		}

		public override bool CanBeObject {
			get { return (true); }
		}

		public Function Value { get; private set; }
	}
}
