using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор while (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.2)
	/// </summary>
	public sealed class WhileStatement : IterationStatement {
		private Expression _condition;

		public WhileStatement(int lineNo, ILabelSet labelSet)
			: base(StatementType.While, lineNo, labelSet) {
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("while (")
				.Append(Condition)
				.AppendLine(") {");
			Statement.AppendTo(output, indent + '\t');
			output.Append(indent)
				.AppendLine("}");
		}

		internal override void DoEmit(CompilingContext context) {
			_condition.CompileBy(context.Compiler, false);
			context.Compiler.Emitter.Emit(OpCode.GotoIfFalse, context.EndLabel);
			Statement.CompileBy(context.Compiler);
			context.Compiler.Emitter.Emit(OpCode.Goto, context.StartLabel);
		}

		public Expression Condition {
			get { return (_condition); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_condition == null);
				_condition = value;
			}
		}
	}
}
