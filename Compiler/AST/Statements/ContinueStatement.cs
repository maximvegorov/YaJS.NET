using System.Diagnostics.Contracts;

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

		public string TargetLabel {
			get { return (_targetLabel); }
		}

		public IterationStatement Target { get; private set; }
	}
}
