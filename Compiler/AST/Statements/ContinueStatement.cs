﻿using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор continue (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.7)
	/// </summary>
	internal sealed class ContinueStatement : Statement {
		private Statement _target;

		public ContinueStatement(Statement parent, Statement target)
			: base(parent, StatementType.Continue) {
			Contract.Requires(target != null);
			_target = target;
		}

		public override bool IsTarget(Statement target) {
			return (object.ReferenceEquals(_target, target));
		}
	}
}
