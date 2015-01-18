using System;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST.Statements;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Тип оператора
	/// </summary>
	public enum StatementType {
		Compound,
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
		While,
		Reference
	}

	/// <summary>
	/// Представляет в AST дереве оператор. Является базовым классом для всех операторов
	/// </summary>
	public abstract class Statement {
		internal static readonly ILabelSet EmptyLabelSet = new EmptyLabelSet();

		protected Statement(Statement parent, StatementType type) {
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

		protected virtual bool IsTarget(Statement target) {
			Contract.Requires(target != null);
			return (false);
		}

		protected virtual void RegisterAsExitPoint(Statement exitPoint) {
			Contract.Requires(exitPoint != null);
		}

		/// <summary>
		/// Зарегистрировать оператор как точку выхода. Используется для правильной обработки блоков finally оператора try
		/// </summary>
		internal void RegisterAsExitPoint() {
			for (var current = Parent; current != null && !current.IsTarget(this); current = current.Parent)
				current.RegisterAsExitPoint(this);
		}

		public Statement WrapToBlock() {
			Contract.Requires(!(this is CompoundStatement));
			var result = new BlockStatement(Parent, LineNo);
			Parent = result;
			result.AddStatement(this);
			return (result);
		}

		protected internal virtual void InsertBefore(Statement position, Statement newStatement) {
			throw new NotSupportedException();
		}

		public void InsertBefore(Statement newStatement) {
			Contract.Requires(newStatement != null);
			Contract.Requires(Parent != null);
			Parent.InsertBefore(this, newStatement);
		}

		internal virtual void CompileBy(FunctionCompiler compiler) {
			Contract.Requires(compiler != null);
			throw new NotSupportedException();
		}

		/// <summary>
		/// Оператор содержащий данный оператор или null
		/// </summary>
		public Statement Parent { get; internal set; }

		/// <summary>
		/// Тип оператора
		/// </summary>
		public StatementType Type { get; private set; }

		/// <summary>
		/// Строка на которой начинается оператор
		/// </summary>
		public abstract int LineNo { get; }
	}
}
