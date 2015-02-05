using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Псевдооператор используемый для ссылки на блоки finally перед точками выхода из блоков try
	/// </summary>
	public sealed class ReferenceStatement : Statement {
		private readonly Statement _reference;

		public ReferenceStatement(Statement reference)
			: base(StatementType.Reference) {
			Contract.Requires(reference != null);
			_reference = reference;
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.AppendLine("{");
			_reference.AppendTo(output, indent + '\t');
			output.Append(indent)
				.AppendLine("}");
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
