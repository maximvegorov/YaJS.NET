using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for-in (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.4)
	/// </summary>
	internal sealed class ForInStatement : IterationStatement {
		private Expression _enumerable;
		private string _variableName;

		public ForInStatement(
			Statement parent,
			int lineNo,
			string variableName,
			Expression enumerable,
			ILabelSet labelSet
			) : base(parent, StatementType.ForIn, lineNo, labelSet) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(enumerable != null);
			_variableName = variableName;
			_enumerable = enumerable;
		}
	}
}
