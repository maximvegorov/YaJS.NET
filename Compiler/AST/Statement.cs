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

		public Statement(Statement parent, StatementType type) {
			Parent = parent;
			Type = type;
		}

		public virtual bool IsBreakTarget(string targetLabel) {
			Contract.Requires(targetLabel != null);
			return (false);
		}
		public virtual bool IsContinueTarget(string targetLabel) {
			Contract.Requires(targetLabel != null);
			return (false);
		}
		public virtual bool CanContainReturn() {
			return (Parent != null ? Parent.CanContainReturn() : true);
		}

		public virtual bool IsTarget(Statement target) {
			return (false);
		}

		protected virtual void RegisterAsExitPoint(Statement exitPoint) {
		}

		/// <summary>
		/// Зарегистрировать оператор как точку выхода. Используется для правильной обработки блоков finally оператора try
		/// </summary>
		public void RegisterAsExitPoint() {
			for (var current = Parent; current != null && !current.IsTarget(this); current = current.Parent) {
				current.RegisterAsExitPoint(this);
			}
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
