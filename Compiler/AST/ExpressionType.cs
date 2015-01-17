namespace YaJS.Compiler.AST {
	/// <summary>
	/// ��� ���������
	/// </summary>
	public enum ExpressionType {
		Undefined,
		Null,
		BooleanLiteral,
		IntegerLiteral,
		FloatLiteral,
		String,
		Ident,
		ObjectLiteral,
		ArrayLiteral,
		FunctionLiteral,
		This,
		Arguments,
		Eval,
		Grouping,
		Member,
		New,
		Call,
		PostfixInc,
		PostfixDec,
		Delete,
		Void,
		TypeOf,
		Inc,
		Dec,
		Pos,
		Neg,
		BitNot,
		Not,
		Mul,
		Div,
		Mod,
		Plus,
		Minus,
		Shl,
		ShrS,
		ShrU,
		Lt,
		Lte,
		Gt,
		Gte,
		InstanceOf,
		In,
		Eq,
		Neq,
		StrictEq,
		StrictNeq,
		BitAnd,
		BitXor,
		BitOr,
		And,
		Or,
		Conditional,
		SimpleAssign,
		PlusAssign,
		MinusAssign,
		MulAssign,
		DivAssign,
		ModAssign,
		ShlAssign,
		ShrSAssign,
		ShrUAssign,
		BitAndAssign,
		BitXorAssign,
		BitOrAssign,
		Sequence
	};
}