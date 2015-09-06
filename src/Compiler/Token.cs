using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	public enum TokenType {
		Unknown, // неизвестный

		Ident, // идентификатор

		// Ключевые слова
		Break, // break
		Case, // case
		Catch, // catch
		Continue, // continue
		Debugger, // debugger
		Default, // default
		Delete, // delete
		Do, // do
		Else, // else
		False, // false
		Finally, // finally
		For, // for
		Function, // function
		If, // if
		In, // in
		InstanceOf, // instanceof
		Null, // null
		New, // new
		Return, // return
		Switch, // switch
		This, // this
		Throw, // throw
		True, // true
		Try, // try
		Typeof, // typeof
		Var, // var
		Void, // void
		While, // while
		With, // with

		// Слова, зарезервированные для использования в будущем
		Class, // class
		Enum, // enum
		Extends, // extends
		Super, // super
		Const, // const
		Export, // export
		Import, // import
		Implements, // implements
		Let, // let
		Private, // private
		Public, // public
		Yield, // yield
		Interface, // interface
		Package, // package
		Protected, // protected
		Static, // static

		// Слова, зарезервированные в данной реализации
		Undefined, // undefined
		Eval, // eval
		Arguments, // arguments

		// Знаки пунктуации
		LCurlyBrace, // {
		RCurlyBrace, // }
		LParenthesis, // (
		RParenthesis, // )
		LBracket, // [
		RBracket, // ]
		Dot, // .
		Semicolon, // ;
		Comma, // ,
		Lt, // <
		Lte, // <=
		Gt, // >
		Gte, // >=
		Eq, // ==
		Neq, // !=
		StrictEq, // ===
		StrictNeq, // !==
		Plus, // +
		Minus, // -
		Star, // *
		Slash, // /
		Mod, // %
		Inc, // ++
		Dec, // --
		Shl, // <<
		ShrS, // >>
		ShrU, // >>>
		BitNot, // ~
		BitAnd, // &
		BitXor, // ^
		BitOr, // |
		Not, // !
		And, // &&
		Or, // ||
		QuestionMark, // ?
		Colon, // :
		Assign, // =
		PlusAssign, // +=
		MinusAssign, // -=
		StarAssign, // *=
		SlashAssign, // /=
		ModAssign, // %=
		ShlAssign, // <<=
		ShrSAssign, // >>=
		ShrUAssign, // >>>=
		BitAndAssign, // &=
		BitXorAssign, // ^=
		BitOrAssign, // |=

		Integer, // целое число
		HexInteger, // шестнацатеричное целое число
		Float, // вещественное число

		String // строка
	}

	public struct TokenPosition {
		private readonly int _columnNo;
		private readonly int _lineNo;

		public TokenPosition(int lineNo, int columnNo) {
			Contract.Requires(lineNo > 0);
			Contract.Requires(columnNo > 0);
			_lineNo = lineNo;
			_columnNo = columnNo;
		}

		public int LineNo { get { return (_lineNo); } }
		public int ColumnNo { get { return (_columnNo); } }
	}

	public sealed class Token {
		public void SetUnknown() {
			Type = TokenType.Unknown;
			IsAfterLineTerminator = false;
			Value = null;
		}

		public override string ToString() {
			if (string.IsNullOrEmpty(Value))
				return (Type.ToString());
			return (string.Format("{0} ({1})", Type, Value));
		}

		public Token Clone() {
			return ((Token)MemberwiseClone());
		}

		/// <summary>
		/// Тип лексемы
		/// </summary>
		public TokenType Type { get; set; }

		/// <summary>
		/// Позиция в потоке символов
		/// </summary>
		public TokenPosition StartPosition { get; set; }

		/// <summary>
		/// Необходимо для автоматической расстановки ; (см. http://ecma-international.org/ecma-262/5.1/#sec-7.9)
		/// </summary>
		public bool IsAfterLineTerminator { get; set; }

		/// <summary>
		/// Сама лексема
		/// </summary>
		public string Value { get; set; }
	}
}
