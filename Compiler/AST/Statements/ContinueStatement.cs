using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор continue (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.7)
	/// </summary>
	public sealed class ContinueStatement : LanguageStatement {
		private readonly IterationStatement _target;

		public ContinueStatement(Statement parent, int lineNo, IterationStatement target)
			: base(parent, StatementType.Continue, lineNo) {
			Contract.Requires(target != null);
			_target = target;
		}

		protected override bool IsTarget(Statement target) {
			return (ReferenceEquals(_target, target));
		}

		public IterationStatement Target { get { return (_target); } }
	}
}
