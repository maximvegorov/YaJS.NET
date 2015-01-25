using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор представляющий тело функции
	/// </summary>
	public sealed class FunctionBodyStatement : CompoundStatement {
		private readonly List<TryStatement> _tryStatements;

		public FunctionBodyStatement()
			: base(1) {
			_tryStatements = new List<TryStatement>();
		}

		internal void RegisterTryStatement(TryStatement tryStatement) {
			Contract.Requires(tryStatement != null);
			_tryStatements.Add(tryStatement);
		}

		internal override void Preprocess(Function function) {
			base.Preprocess(function);
			/// Копируем операторы блока finally перед каждой точкой выхода из блока try
			foreach (var tryStatement in _tryStatements) {
				if (tryStatement.FinallyBlock == null || tryStatement.TryBlock.ExitPoints.Count == 0)
					continue;
				var finallyBlock = tryStatement.FinallyBlock;
				foreach (var exitPoint in tryStatement.TryBlock.ExitPoints)
					exitPoint.InsertBefore(new ReferenceStatement(finallyBlock));
			}
		}
	}
}
