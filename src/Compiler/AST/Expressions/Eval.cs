﻿using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class Eval : Expression {
		private const string EvalLiteral = "eval";

		internal Eval()
			: base(ExpressionType.Eval) {
		}

		public override string ToString() {
			return (EvalLiteral);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdLocal, EvalLiteral);
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
