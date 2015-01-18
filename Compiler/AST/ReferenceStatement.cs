using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Псевдооператор используемый для ссылки на блоки finally в перед точками выхода блоков try
	/// </summary>
	internal sealed class ReferenceStatement : Statement {
		private readonly Statement _reference;

		public ReferenceStatement(Statement parent, Statement reference)
			: base(parent, StatementType.Reference) {
			Contract.Requires(reference != null);
			_reference = reference;
		}

		public override int LineNo { get { return (_reference.LineNo); } }
	}
}
