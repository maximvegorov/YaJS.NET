namespace YaJS.Runtime {
	/// <summary>
	/// Код инструкции
	/// </summary>
	public enum OpCode : byte {
		Nop, // Ничего не делать
		Break, // Тоже самое что Nop, но используется для разделения операторов

		LdUndefined, // Загрузить в стек undefined
		LdNull, // Загрузить в стек null
		LdBoolean, // Загрузить в стек boolean
		LdInteger, // Загрузить в стек integer
		LdFloat, // Загрузить в стек float
		LdString, // Загрузить в стек string

		LdThis, // Загрузить в стек this

		LdLocalFunc, // Загрузить в стек ссылку на локальную функцию

		LdLocal, // Загрузить в стек значение локальной переменной
		StLocal, // Извлечь из стека значение и сохранить в локальной переменной

		IsMember, // Проверить содержит ли объект S[top] свойство S[top]

		MakeRef, // Создать ссылку на свойство S[top] объекта S[top - 1]

		LdMember, // Загрузить в стек значение свойства S[top] объекта S[top - 1]
		StMember, // Извлечь из стека значение S[top] и сохранить в свойстве S[top - 1] объекта S[top - 2]
		StMemberDup,
		// Извлечь из стека значение S[top] и сохранить в свойстве S[top - 1] объекта S[top - 2], сохранив в стеке исходное значение S[top]

		LdMemberByRef, // Загрузить в стек значение по ссылке S[top]
		StMemberByRef, // Извлечь из стека значение S[top] по ссылке S[top - 1]
		StMemberByRefDup, // Извлечь из стека значение S[top] по ссылке S[top - 1], сохранив в стеке исходное значение S[top]

		DelMember, // Удалить из объекта S[top - 1] свойство S[top]

		Dup, // Дублировать S[top]
		Dup2, // Дублировать S[top] и S[top - 1]
		Pop, // Вытолкнуть S[top]
		Pop2, // Вытолкнуть S[top] и S[top - 1]
		Swap, // Переставить местами S[top] и S[top - 1]
		SwapDup, // Переставить местами S[top] и S[top - 1] и дублировать исходный S[top]

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
		Rethrow, // Продолжить поиск обработчика текущего исключения, если объекта исколючения нет то равносильно Nop

		NewObj, // Создать объект c помощью конструктора

		MakeEmptyObject, // Создать пустой объект
		MakeObject, // Создать объект из инициализатора
		MakeEmptyArray, // Создать пустой массив
		MakeArray, // Создать массив из инициализатора

		Call, // Вызвать функцию
		CallMember, // Вызвать метод объекта

		Return, // Вернуться из функции

		GetEnumerator, // Вернуть перечислитель
		MoveNext, // Перейти к следующему элементу перечислителя

		CastToPrimitive, // Преобразовать объект в примитивное значение

		Pos, // Number(S[top])
		Neg, // -S[top]

		Inc, // S[top] + 1
		Dec, // S[top] - 1
		Plus, // S[top - 1] + S[top]
		Minus, // S[top - 1] - S[top] 
		Mul, // S[top - 1] * S[top] 
		IntDiv, // S[top - 1] / S[top] (Целочисленное)
		FltDiv, // S[top - 1] / S[top] (Вещественное)
		Mod, // S[top - 1] % S[top] 

		BitNot, // ~S[top]
		BitAnd, // S[top - 1] & S[top] 
		BitOr, // S[top - 1] | S[top]
		BitXor, // S[top - 1] ^ S[top] 

		Shl, // S[top - 1] << S[top]
		ShrS, // S[top - 1] >> S[top]
		ShrU, // S[top - 1] >>> S[top]

		Not, // !S[top]
		And, // S[top - 1] && S[top]
		Or, // S[top - 1] || S[top]

		ConvEq, // S[top - 1] == S[top] (Только для примитивных типов)
		StrictEq, // S[top - 1] === S[top]
		ConvNeq, // S[top - 1] != S[top] (Только для примитивных типов)
		StrictNeq, // S[top - 1] !== S[top]

		Lt, // S[top - 1] < S[top]
		Lte, // S[top - 1] <= S[top]
		Gt, // S[top - 1] > S[top]
		Gte, // S[top - 1] >= S[top]

		InstanceOf, // S[top - 1] instanceof S[top]
		TypeOf // typeof S[top]
	}
}
