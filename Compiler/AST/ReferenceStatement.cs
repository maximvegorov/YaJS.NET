using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Псевдооператор используемый для ссылки на блоки finally перед точками выхода из блоков try
	/// </summary>
	internal sealed class ReferenceStatement : Statement {
		private readonly Statement _reference;

		public ReferenceStatement(Statement parent, Statement reference)
			: base(parent, StatementType.Reference) {
			Contract.Requires(reference != null);
			_reference = reference;
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			var savedParent = _reference.Parent;
			_reference.Parent = Parent;
			try {
				_reference.CompileBy(compiler);
			}
			finally {
				_reference.Parent = savedParent;
			}
		}

		public override int LineNo { get { return (_reference.LineNo); } }
	}
}
