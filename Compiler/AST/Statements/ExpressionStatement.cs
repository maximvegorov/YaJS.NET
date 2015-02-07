using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор выражение (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.4)
	/// </summary>
	public sealed class ExpressionStatement : LanguageStatement {
		private readonly Expression _expression;

		public ExpressionStatement(int lineNo, Expression expression)
			: base(StatementType.Expression, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append(Expression)
				.AppendLine(";");
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			if (compiler.IsEvalMode) {
				// В случае режима eval необходимо оставлять результат последней операции в стеке вычислений,
				// но прежде необходимо вытолкнуть предыдущий результат
				compiler.Emitter.Emit(OpCode.Pop);
				_expression.CompileBy(compiler, false);
			}
			else
				_expression.CompileBy(compiler, true);
			compiler.MarkEndOfStatement();
		}

		public Expression Expression { get { return (_expression); } }
	}
}
