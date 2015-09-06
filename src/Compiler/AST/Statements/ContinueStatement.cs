using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Compiler.Emitter;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор continue (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.7)
	/// </summary>
	public sealed class ContinueStatement : LanguageStatement {
		private readonly string _targetLabel;

		public ContinueStatement(int lineNo, string targetLabel)
			: base(StatementType.Continue, lineNo) {
			Contract.Requires(targetLabel != null);
			_targetLabel = targetLabel;
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("continue");
			if (!string.IsNullOrEmpty(_targetLabel)) {
				output.Append(' ')
					.Append(_targetLabel);
			}
			output.AppendLine(";");
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			var current = Parent;
			while (current != null && !current.IsContinueTarget(_targetLabel)) {
				current.RegisterExitPoint(this);
				current = current.Parent;
			}
			var target = current as IterationStatement;
			if (target == null)
				Errors.ThrowUnreachableLabel(LineNo, _targetLabel);
			Target = target;
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			Label targetStartLabel;
			if (!compiler.StatementStarts.TryGetValue(Target, out targetStartLabel))
				Errors.ThrowInternalError();
			compiler.Emitter.Emit(OpCode.Goto, targetStartLabel);
		}

		public string TargetLabel { get { return (_targetLabel); } }
		public IterationStatement Target { get; private set; }
	}
}
