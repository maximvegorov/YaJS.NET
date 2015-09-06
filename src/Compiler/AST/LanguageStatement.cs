using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Базовый класс для всех операторов языка Javascript, согласно спецификации (См.
	/// http://www.ecma-international.org/ecma-262/5.1/#sec-12)
	/// </summary>
	public abstract class LanguageStatement : Statement {
		private readonly int _lineNo;

		protected LanguageStatement(StatementType type, int lineNo)
			: base(type) {
			Contract.Requires(lineNo >= 1);
			_lineNo = lineNo;
		}

		public override int LineNo { get { return (_lineNo); } }
	}
}
