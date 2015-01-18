﻿namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Блочный оператор (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.3)
	/// </summary>
	public class BlockStatement : CompoundStatement {
		public BlockStatement(Statement parent, int lineNo)
			: base(parent, StatementType.Compound, lineNo) {
		}
	}
}
