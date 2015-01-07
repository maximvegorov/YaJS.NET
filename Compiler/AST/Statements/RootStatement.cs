using System.Collections.Generic;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдооператор. Используется в качестве корневого для операторов программы и функций
	/// </summary>
	internal sealed class RootStatement : CompoundStatement {
		private bool _canContainReturn;

		public RootStatement(bool canContainReturn)
			: base(null, StatementType.Root) {
			_canContainReturn = canContainReturn;
		}

		public override bool CanContainReturn() {
			return (_canContainReturn);
		}
	}
}
