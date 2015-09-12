using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор do-while (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6.1)
	/// </summary>
	public sealed class DoWhileStatement : IterationStatement {
		private Expression _condition;

		public DoWhileStatement(int lineNo, ILabelSet labelSet)
			: base(StatementType.DoWhile, lineNo, labelSet) {
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.AppendLine("do {");
			Statement.AppendTo(output, indent + '\t');
			output.Append(indent)
				.Append("} while (")
				.Append(Condition)
				.AppendLine(")");
		}

		internal override void DoEmit(CompilingContext context) {
			Statement.CompileBy(context.Compiler);
			_condition.CompileBy(context.Compiler, false);
			context.Compiler.Emitter.Emit(OpCode.GotoIfTrue, context.StartLabel);
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
