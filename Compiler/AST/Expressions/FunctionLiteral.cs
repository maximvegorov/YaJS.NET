﻿using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class FunctionLiteral : Expression {
		private readonly Function _function;

		public FunctionLiteral(Function function)
			: base(ExpressionType.FunctionLiteral) {
			Contract.Requires(function != null);
			_function = function;
		}

		public override string ToString() {
			return (_function.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
