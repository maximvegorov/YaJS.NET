using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор for-in (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.4)
	/// </summary>
	public sealed class ForInStatement : IterationStatement {
		private readonly string _variableName;
		private readonly Expression _enumerable;

		public ForInStatement(int lineNo, string variableName, Expression enumerable, ILabelSet labelSet)
			: base(StatementType.ForIn, lineNo, labelSet) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(enumerable != null);
			_variableName = variableName;
			_enumerable = enumerable;
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("for (")
				.Append(_variableName)
				.Append(" in ")
				.Append(_enumerable)
				.AppendLine(") {");
			Statement.AppendTo(output, indent + '\t');
			output.Append(indent)
				.AppendLine("}");
		}

		internal override void DoEmitProlog(FunctionCompiler compiler) {
			_enumerable.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.GetEnumerator);
		}

		internal override void DoEmit(CompilingContext context) {
			context.Compiler.Emitter.Emit(OpCode.MoveNext, _variableName);
			context.Compiler.Emitter.Emit(OpCode.GotoIfFalse, context.EndLabel);
			Statement.CompileBy(context.Compiler);
			context.Compiler.Emitter.Emit(OpCode.Goto, context.StartLabel);
		}

		internal override void DoEmitEpilog(FunctionCompiler compiler) {
			compiler.Emitter.Emit(OpCode.Pop);
		}

		public string VariableName { get { return (_variableName); } }
		public Expression Enumerable { get { return (_enumerable); } }
	}
}
