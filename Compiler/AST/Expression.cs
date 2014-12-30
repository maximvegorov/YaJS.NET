namespace YaJS.Compiler.AST {
	/// <summary>
	/// Является базовым классом для всех выражений
	/// </summary>
	public abstract class Expression {
		/// <summary>
		/// Является ли выражение ссылкой (можно использовать в левой части оператора присваивания)
		/// </summary>
		public virtual bool IsReference { get { return (false); } }
		/// <summary>
		/// Применим ли к выражению MemberOperator
		/// </summary>
		public virtual bool CanHaveMembers { get { return (false); } }
		/// <summary>
		/// Будет ли выражение ссылкой после применения MemberOperator
		/// </summary>
		public virtual bool CanHaveMutableMembers { get { return (false); } }
		/// <summary>
		/// Применим ли к выражению NewOperator
		/// </summary>
		public virtual bool CanBeConstructor { get { return (false); } }
		/// <summary>
		/// Применим ли к выражению CallOperator
		/// </summary>
		public virtual bool CanBeFunction { get { return (false); } }
		/// <summary>
		/// Применим ли к выражению DeleteOperator
		/// </summary>
		public virtual bool CanBeDeleted { get { return (false); } }
	}
}
