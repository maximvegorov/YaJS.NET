using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Базовый класс для всех операторов, которые могут иметь набор меток do/while, for, while, switch
	/// </summary>
	public abstract class LabellableStatement : LanguageStatement {
		private readonly ILabelSet _labelSet;

		protected LabellableStatement(StatementType type, int lineNo, ILabelSet labelSet)
			: base(type, lineNo) {
			Contract.Requires(labelSet != null);
			_labelSet = labelSet.UnionWith(string.Empty);
		}

		internal override bool IsBreakTarget(string targetLabel) {
			return (_labelSet.Contains(targetLabel));
		}

		internal override bool IsContinueTarget(string targetLabel) {
			return (_labelSet.Contains(targetLabel));
		}

		public ILabelSet LabelSet { get { return (_labelSet); } }
	}
}
