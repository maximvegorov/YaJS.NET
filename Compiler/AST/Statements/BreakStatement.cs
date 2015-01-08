using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор break (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.8)
	/// </summary>
	internal sealed class BreakStatement : Statement {
		private Statement _target;

		public BreakStatement(Statement parent, Statement target)
			: base(parent, StatementType.Break) {
			Contract.Requires(target != null);
			_target = target;
		}

		public override bool IsTarget(Statement target) {
			return (object.ReferenceEquals(_target, target));
		}
	}
}
