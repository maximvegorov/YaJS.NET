using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор break (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.8)
	/// </summary>
	internal sealed class BreakStatement : Statement {
		private readonly Statement _target;

		public BreakStatement(Statement parent, int lineNo, Statement target)
			: base(parent, StatementType.Break, lineNo) {
			Contract.Requires(target != null);
			_target = target;
		}

		protected override bool IsTarget(Statement target) {
			return (ReferenceEquals(_target, target));
		}
	}
}
