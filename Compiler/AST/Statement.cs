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
		CaseClause,
		CaseClauseBlock,
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

		protected Statement(StatementType type) {
			Type = type;
		}

		internal virtual bool IsBreakTarget(string targetLabel) {
			Contract.Requires(targetLabel != null);
			return (false);
		}

		internal virtual bool IsContinueTarget(string targetLabel) {
			Contract.Requires(targetLabel != null);
			return (false);
		}

		internal virtual void RegisterExitPoint(Statement exitPoint) {
			Contract.Requires(exitPoint != null);
		}

		public Statement WrapToBlock() {
			Contract.Requires(Type != StatementType.Compound);
			var result = new BlockStatement(LineNo);
			Parent = result;
			result.Append(this);
			return (result);
		}

		protected internal virtual void InsertBefore(Statement position, Statement newStatement) {
			throw new NotSupportedException();
		}

		public void InsertBefore(Statement newStatement) {
			Contract.Requires(newStatement != null);
			Contract.Requires(Parent != null);
			Contract.Ensures(newStatement.Parent == Parent);
			Parent.InsertBefore(this, newStatement);
		}

		protected virtual void Remove(Statement child) {
			throw new NotSupportedException();
		}

		public void Remove() {
			Contract.Requires(Parent != null);
			Parent.Remove(this);
			Parent = null;
		}

		internal virtual void Preprocess(Function function) {
			Contract.Requires(function != null);
		}

		internal virtual void CompileBy(FunctionCompiler compiler) {
			Contract.Requires(compiler != null);
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
