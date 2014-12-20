namespace YaJS.Compiler.Parser {
	public enum TokenType {
		Unknown,		// unknown

		Ident,			// identifier

		// Ключевые слова
		Break,			// break
		Case,			// case
		Catch,			// catch
		Continue,		// continue
		Debugger,		// debugger
		Default,		// default
		Delete,			// delete
		Do,				// do
		Else,			// else
		False,			// false
		Finally,		// finally
		For,			// for
		Function,		// function
		If,				// if
		In,				// in
		InstanceOf,		// instanceof
		Null,			// null
		New,			// new
		Return,			// return
		Switch,			// switch
		This,			// this
		Throw,			// throw
		True,			// true
		Try,			// try
		Typeof,			// typeof
		Var,			// var
		Void,			// void
		While,			// while
		With,			// with

		// Слова, зарезервированные для использования в будущем
		Class,			// class
		Enum,			// enum
		Extends,		// extends
		Super,			// super
		Const,			// const
		Export,			// export
		Import,			// import
		Implements,		// implements
		Let,			// let
		Private,		// private
		Public,			// public
		Yield,			// yield
		Interface,		// interface
		Package,		// package
		Protected,		// protected
		Static,			// static

		// Знаки пунктуации
		LCurlyBrace,	// {
		RCurlyBrace,	// }
		LParenthesis,	// (
		RParenthesis,	// )
		LBracket,		// [
		RBracket,		// ]
		Dot,			// .
		Semicolon,		// ;
		Comma,			// ,
		Lt,				// <
		Lte,			// <=
		Gt,				// >
		Gte,			// >=
		Eq,				// ==
		Neq,			// !=
		StrictEq,		// ===
		StrictNeq,		// !==
		Plus,			// +
		Minus,			// -
		Star,			// *
		Slash,			// /
		Mod,			// %
		Inc,			// ++
		Dec,			// --
		Shl,			// <<
		ShrS,			// >>
		ShrU,			// >>>
		BitNot,			// ~
		BitAnd,			// &
		BitOr,			// |
		BitXor,			// ^
		Not,			// !
		And,			// &&
		Or,				// ||
		QuestionMark,	// ?
		Colon,			// :
		Assign,			// =
		PlusAssign,		// +=
		MinusAssign,	// -=
		StarAssign,		// *=
		SlashAssign,	// /=
		ModAssign,		// %=
		ShlAssign,		// <<=
		ShrSAssign,		// >>=
		ShrUAssign,		// >>>=
		BitAndAssign,	// &=
		BitOrAssign,	// |=
		BitXorAssign,	// ^=

		Number,			// integer or float number

		String			// string
	}

	public sealed class Token {
		public void SetUnknown() {
			Type = TokenType.Unknown;
			IsAfterLineTerminator = false;
			Value = null;
		}

		public TokenType Type { get; set; }
		/// <summary>
		/// Необходимо для автоматической расстановки ;
		/// </summary>
		public bool IsAfterLineTerminator { get; set; }
		public string Value { get; set; }
	}
}
