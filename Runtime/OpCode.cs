namespace YaJS.Runtime {
	/// <summary>
	/// Код инструкции
	/// </summary>
	public enum OpCode : byte {
		Nop, // Ничего не делать

		LdUndefined, // Загрузить в стек undefined
		LdNull, // Загрузить в стек null
		LdBoolean, // Загрузить в стек boolean
		LdInteger, // Загрузить в стек integer
		LdFloat, // Загрузить в стек float
		LdString, // Загрузить в стек string

		LdThis, // Загрузить в стек this

		LdLocal, // Загрузить в стек значение локальной переменной
		LdLocalFunc, // Загрузить в стек ссылку на локальную функцию
		StLocal, // Извлечь из стека значение и сохранить в локальной переменной
		DelLocal, // Удалить локальную переменную

		IncLocal, // Увеличить значение локальной переменной на 1
		DecLocal, // Уменьшить значение локальной переменной на 1

		IsMember, // Проверить содержит ли объект S[top - 1] свойство S[top]
		LdMember, // Загрузить в стек значение свойства объекта S[top - 1].S[top]
		StMember, // Извлечь из стека значение и сохранить в свойстве объекта S[top - 1].S[top]
		DelMember, // Удалить из объекта S[top] свойство S[top - 1]

		IncMember, // Увеличить значение свойства объекта на 1
		DecMember, // Уменьшить значение свойства объекта на 1

		Dup, // Продублировать S[top]
		Dup2, // Продублировать S[top] и S[top - 1]
		Pop, // Вытолкнуть из стека значение S[top]
		Pop2, // Вытолкнуть из стека значения S[top] и S[top - 1]
		Swap, // Переставляет местами S[top] и S[top - 1]

		Goto, // Безусловный переход
		GotoIfTrue, // Перейти если S[top] истинно
		GotoIfFalse, // Перейти если S[top] ложно

		Switch, // Перейти по таблице переходов с учетом значения S[top]

		BeginScope, // Создать новую область локальных переменных
		EndScope, // Вернуться к предыдущей области локальных переменных

		EnterTry, // Войти в блок try
		LeaveTry, // Выйти из блока try

		EnterCatch, // Войти в блок catch
		LeaveCatch, // Выйти из блока catch

		Throw, // Выбросить исключение (throw S[top])
		Rethrow, // Продолжить поиск обработчика текущего исключения

		NewObj, // Создать объект
		MakeEmptyObject, // Создать пустой объект
		MakeObject, // Создать объект из инициализатора
		MakeEmptyArray, // Создать пустой массив
		MakeArray, // Создать массив из инициализатора

		Call, // Вызвать функцию
		CallMember, // Вызвать метод объекта

		Return, // Вернуться из функции

		GetEnumerator, // Вернуть перечислитель
		EnumMoveNext, // Перейти к следующему элементу перечислителя

		CastToPrimitive, // Преобразовать объект в примитивное значение

		Pos, // Number(S[top])
		Neg, // -S[top]

		Inc, // S[top] + 1
		Dec, // S[top] - 1
		Plus, // S[top] + S[top - 1]
		Minus, // S[top] - S[top - 1]
		Mul, // S[top] * S[top - 1]
		IntDiv, // S[top]  S[top - 1] (Целочисленное)
		FltDiv, // S[top] / S[top - 1] (Вещественное)
		Mod, // S[top] % S[top - 1]

		BitNot, // ~S[top]
		BitAnd, // S[top] & S[top - 1]
		BitOr, // S[top] | S[top - 1]
		BitXor, // S[top] ^ S[top - 1]

		Shl, // S[top] << S[top - 1]
		ShrS, // S[top] >> S[top - 1]
		ShrU, // S[top] >>> S[top - 1]

		Not, // !S[top]
		And, // S[top] && S[top - 1]
		Or, // S[top] || S[top - 1]

		ConvEq, // S[top] == S[top - 1] (Только для примитивных типов)
		StrictEq, // S[top] === S[top - 1]
		ConvNeq, // S[top] != S[top - 1]
		StrictNeq, // S[top] !== S[top - 1] (Только для примитивных типов)

		Lt, // S[top] < S[top - 1]
		Lte, // S[top] <= S[top - 1]
		Gt, // S[top] > S[top - 1]
		Gte, // S[top] >= S[top - 1]

		InstanceOf, // S[top] instanceof S[top - 1]
		TypeOf // typeof S[top]
	}
}
