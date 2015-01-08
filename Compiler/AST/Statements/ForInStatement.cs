using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for-in (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.4)
	/// </summary>
	internal sealed class ForInStatement : IterationStatement {
		private string _variableName;
		private Expression _enumerable;

		public ForInStatement(
			Statement parent,
			string variableName,
			Expression enumerable,
			ILabelSet labelSet
		) : base(parent, StatementType.ForIn, labelSet) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(enumerable != null);
			_variableName = variableName;
			_enumerable = enumerable;
		}
	}
}
