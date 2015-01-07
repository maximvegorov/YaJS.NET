using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Блочный оператор (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.1)
	/// </summary>
	public sealed class BlockStatement : CompoundStatement {
		public BlockStatement(Statement parent)
			: base(parent, StatementType.Block) {
		}
	}
}
