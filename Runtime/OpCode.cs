namespace YaJS.Runtime {
	/// <summary>
	/// Код инструкции
	/// </summary>
	public enum OpCode : byte {
		Nop,			// Ничего не делать

		LdUndefined,	// Загрузить в стек undefined
		LdNull,			// Загрузить в стек null
		LdBoolean,		// Загрузить в стек boolean
		LdInteger,		// Загрузить в стек integer
		LdFloat,		// Загрузить в стек float
		LdString,		// Загрузить в стек string

		LdThis,			// Загрузить в стек this

		DeclLocal,		// Выделить место для локальной переменной

		LdLocal,		// Загрузить в стек значение локальной переменной
		LdLocalFunc,	// Загрузить в стек ссылку на локальную функцию
		StLocal,		// Извлечь из стека значение и сохранить в локальной переменной

		IsMember,		// Проверить содержит ли объект S[top] свойство S[top - 1]
		LdMember,		// Загрузить в стек значение свойства объекта
		StMember,		// Извлечь из стека значение и сохранить в свойстве объекта
		DelMember,		// Удалить из объекта S[top] свойство S[top - 1]

		Dup,			// Продублировать вершину стека
		Pop,			// Извлечь из стека значение

		Goto,			// Безусловный переход
		GotoIfTrue,		// Перейти если S[top] истинно
		GotoIfFalse,	// Перейти если S[top] ложно

		BeginScope,		// Создать новую область локальных переменных
		EndScope,		// Вернуться к предыдущей области локальных переменных

		BeginTry,		// Войти в блок try
		EndTry,			// Выйти из блока try
		EndCatch,		// Выйти из блока catch
		EndFinally,		// Выйти из блока finally

		Throw,			// Выбросить исключение (throw S[top])

		NewObj,			// Создать объект
		MakeObject,		// Создать объект из инициализатора
		MakeArray,		// Создать массив из инициализатора

		Call,			// Вызвать функцию
		CallMember,		// Вызвать метод объекта

		Return,			// Вернуться из функции

		Neg,			// -S[top]

		Plus,			// S[top] + S[top - 1]
		Minus,			// S[top] - S[top - 1]
		Mul,			// S[top] * S[top - 1]
		Div,			// S[top] / S[top - 1]
		Mod,			// S[top] % S[top - 1]

		BitNot,			// ~S[top]
		BitAnd,			// S[top] & S[top - 1]
		BitOr,			// S[top] | S[top - 1]
		BitXor,			// S[top] ^ S[top - 1]

		Shl,			// S[top] << S[top - 1]
		ShrS,			// S[top] >> S[top - 1]
		ShrU,			// S[top] >>> S[top - 1]

		Not,			// !S[top]
		And,			// S[top] && S[top - 1]
		Or,				// S[top] || S[top - 1]

		Eq,				// S[top] == S[top - 1]
		StrictEq,		// S[top] === S[top - 1]
		Neq,			// S[top] != S[top - 1]
		StrictNeq,		// S[top] !== S[top - 1]

		Cmp,			// Сравнить S[top] с S[top - 1], -1 если S[top] < S[top - 1], 0 если S[top] = S[top - 1], иначе 1

		Gt,				// S[top] > S[top - 1]
		Gte,			// S[top] >= S[top - 1]
		Lt,				// S[top] < S[top - 1]
		Lte,			// S[top] <= S[top - 1]

		InstanceOf,		// S[top] instanceof S[top - 1]
		TypeOf			// typeof S[top]
	}
}
