using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор break (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.8)
	/// </summary>
	public sealed class BreakStatement : LanguageStatement {
		private readonly LabellableStatement _target;

		public BreakStatement(Statement parent, int lineNo, LabellableStatement target)
			: base(parent, StatementType.Break, lineNo) {
			Contract.Requires(target != null);
			_target = target;
		}

		protected override bool IsTarget(Statement target) {
			return (ReferenceEquals(_target, target));
		}

		public LabellableStatement Target { get { return (_target); } }
	}
}
