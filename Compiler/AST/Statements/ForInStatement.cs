using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for-in (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.4)
	/// </summary>
	public sealed class ForInStatement : IterationStatement {
		private readonly string _variableName;
		private readonly Expression _enumerable;

		public ForInStatement(
			int lineNo,
			string variableName,
			Expression enumerable,
			ILabelSet labelSet
			)
			: base(StatementType.ForIn, lineNo, labelSet) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(enumerable != null);
			_variableName = variableName;
			_enumerable = enumerable;
		}

		public string VariableName { get { return (_variableName); } }
		public Expression Enumerable { get { return (_enumerable); } }
	}
}
