using System;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Тип оператора
	/// </summary>
	public enum StatementType {
		Root,
		Block,
		Break,
		Continue,
		DoWhile,
		Empty,
		Expression,
		If,
		IfElse,
		For,
		ForIn,
		Return,
		Throw,
		Try,
		Switch,
		While
	}

	/// <summary>
	/// Представляет в AST дереве оператор. Является базовым классом для всех операторов
	/// </summary>
	public abstract class Statement {
		internal static readonly ILabelSet EmptyLabelSet = new EmptyLabelSet();
		internal static readonly ILabelSet OneEmptyStringLabelSet = new SingletonLabelSet(string.Empty);

		private ILabelSet _labelSet;

		public Statement(Statement parent, StatementType type, ILabelSet labelSet) {
			Contract.Requires(labelSet != null);
			Parent = parent;
			Type = type;
			_labelSet = labelSet;
		}

		internal bool ContainsLabel(string label) {
			return (_labelSet.Contains(label));
		}
		internal void AddLabel(string label) {
			_labelSet = _labelSet.UnionWith(label);
		}

		public virtual bool CanContainBreak(string target) {
			Contract.Requires(target != null);
			return (Parent != null ? Parent.CanContainBreak(target) : false);
		}
		public virtual bool CanContainContinue(string target) {
			Contract.Requires(target != null);
			return (Parent != null ? Parent.CanContainContinue(target) : false);
		}
		public virtual bool CanContainReturn() {
			return (Parent != null ? Parent.CanContainReturn() : true);
		}

		/// <summary>
		/// Оператор содержащий данный оператор или null
		/// </summary>
		public Statement Parent { get; private set; }
		/// <summary>
		/// Тип оператора
		/// </summary>
		public StatementType Type { get; private set; }
	}
}
