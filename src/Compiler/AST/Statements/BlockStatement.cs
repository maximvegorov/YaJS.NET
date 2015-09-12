using System.Text;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Блочный оператор (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.3)
	/// </summary>
	public class BlockStatement : CompoundStatement {
		public BlockStatement(int lineNo)
			: base(lineNo) {
		}
	}
}
