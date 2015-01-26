using System.Diagnostics.Contracts;
using YaJS.Compiler.Emitter;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор break (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.8)
	/// </summary>
	public sealed class BreakStatement : LanguageStatement {
		private readonly string _targetLabel;

		public BreakStatement(int lineNo, string targetLabel)
			: base(StatementType.Break, lineNo) {
			Contract.Requires(targetLabel != null);
			_targetLabel = targetLabel;
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			var current = Parent;
			while (current != null && !current.IsBreakTarget(_targetLabel)) {
				current.RegisterExitPoint(this);
				current = current.Parent;
			}
			var target = current as LabellableStatement;
			if (target == null)
				Errors.ThrowUnreachableLabel(LineNo, _targetLabel);
			Target = target;
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			Label targetEndLabel;
			if (!compiler.StatementEnds.TryGetValue(Target, out targetEndLabel))
				Errors.ThrowInternalError();
			compiler.Emitter.Emit(OpCode.Goto, targetEndLabel);
		}

		public string TargetLabel { get { return (_targetLabel); } }
		public LabellableStatement Target { get; private set; }
	}
}
