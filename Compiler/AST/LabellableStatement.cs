using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Базовый класс для всех операторов, которые могут иметь набор меток do/while, for, while, switch
	/// </summary>
	internal class LabellableStatement : Statement {
		private ILabelSet _labelSet;

		public LabellableStatement(Statement parent, StatementType type, ILabelSet labelSet)
			: base(parent, type) {
			Contract.Requires(labelSet != null);
			_labelSet = labelSet.UnionWith(string.Empty);
		}

		public override bool IsBreakTarget(string targetLabel) {
			return (_labelSet.Contains(targetLabel));
		}
		public override bool IsContinueTarget(string targetLabel) {
			return (_labelSet.Contains(targetLabel));
		}
	}
}
