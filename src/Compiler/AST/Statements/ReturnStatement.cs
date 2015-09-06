using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор return (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.9)
	/// </summary>
	public sealed class ReturnStatement : LanguageStatement {
		public ReturnStatement(int lineNo, Expression expression)
			: base(StatementType.Return, lineNo) {
			Contract.Requires(expression != null);
			Expression = expression;
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("return ")
				.Append(Expression)
				.AppendLine(";");
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			for (var current = Parent; current != null; current = current.Parent)
				current.RegisterExitPoint(this);
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			Expression.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.Return);
		}

		public Expression Expression { get; private set; }
	}
}
