using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class FunctionLiteral : Expression {
		internal FunctionLiteral(Function function)
			: base(ExpressionType.FunctionLiteral) {
			Contract.Requires(function != null);
			Function = function;
		}

		public override string ToString() {
			return (Function.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdLocalFunc, compiler.Function.Index);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Function Function { get; private set; }
	}
}
