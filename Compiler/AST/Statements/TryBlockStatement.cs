using System.Collections.Generic;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор представляющий TryBlock оператора try (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.14)
	/// </summary>
	public sealed class TryBlockStatement : CompoundStatement {
		private readonly List<Statement> _exitPoints;

		public TryBlockStatement(Statement parent, int lineNo)
			: base(parent, StatementType.Compound, lineNo) {
			_exitPoints = new List<Statement>();
		}

		protected override void RegisterAsExitPoint(Statement exitPoint) {
			_exitPoints.Add(exitPoint);
		}

		public IReadOnlyList<Statement> ExitPoints { get { return (_exitPoints); } }
	}
}
