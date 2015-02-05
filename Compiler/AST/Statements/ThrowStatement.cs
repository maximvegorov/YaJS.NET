using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор throw (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.13)
	/// </summary>
	public sealed class ThrowStatement : LanguageStatement {
		private readonly Expression _expression;

		public ThrowStatement(int lineNo, Expression expression)
			: base(StatementType.Throw, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("throw ")
				.Append(Expression)
				.AppendLine(";");
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			_expression.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.Throw);
		}

		public Expression Expression { get { return (_expression); } }
	}
}
