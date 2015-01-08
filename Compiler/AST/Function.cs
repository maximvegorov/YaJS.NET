using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Представляет в AST дереве функцию
	/// </summary>
	public sealed class Function {
		public Function(
			string name,
			List<string> parameterNames,
			List<string> declaredVariables,
			List<Function> nestedFunctions,
			Statement functionBody,
			bool isDeclaration
		) {
			Contract.Requires(!(isDeclaration && string.IsNullOrEmpty(name)));
			Contract.Requires(parameterNames != null);
			Contract.Requires(declaredVariables != null);
			Contract.Requires(nestedFunctions != null);
			Contract.Requires(functionBody != null);

			Name = name;
			ParameterNames = parameterNames;
			DeclaredVariables = declaredVariables;
			NestedFunctions = nestedFunctions;
			FunctionBody = functionBody;
			IsDeclaration = isDeclaration;

			// Переупорядочить NestedFunctions переместив FunctionDeclaration в начало
			// в порядке следования в исходном тексте. Необходимо для более быстрого
			// нахождения FunctionDeclaration при инициализации LocalScope
			for (var i = 0; i < NestedFunctions.Count; i++) {
				if (NestedFunctions[i].IsDeclaration && i > FunctionDeclarationCount) {
					var t = NestedFunctions[FunctionDeclarationCount];
					NestedFunctions[FunctionDeclarationCount] = NestedFunctions[i];
					NestedFunctions[i] = t;
					NestedFunctions[FunctionDeclarationCount].Index = i;
					FunctionDeclarationCount++;
				}
			}

			// Присвоить индексы всем оставшимся вложенным функциям 
			for (var i = FunctionDeclarationCount; i < NestedFunctions.Count; i++)
				NestedFunctions[i].Index = i;
		}

		/// <summary>
		/// Имя функции. Обязательно для FunctionDeclaration
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Список имен параметров функции
		/// </summary>
		public List<string> ParameterNames { get; private set; }
		/// <summary>
		/// Список объявленных переменных
		/// </summary>
		public List<string> DeclaredVariables { get; private set; }
		/// <summary>
		/// Список вложенных функций
		/// </summary>
		public List<Function> NestedFunctions { get; private set; }
		/// <summary>
		/// Тело функции
		/// </summary>
		public Statement FunctionBody { get; private set; }
		/// <summary>
		/// Является ли функция FunctionDeclaration
		/// </summary>
		public bool IsDeclaration { get; set; }
		/// <summary>
		/// Кол-во FunctionDeclaration в списке NestedFunctions
		/// </summary>
		public int FunctionDeclarationCount { get; private set; }
		/// <summary>
		/// Индекс функции в списке вложенных функций внешней функции (используется для кодогенерации)
		/// </summary>
		public int Index { get; internal set; }
	}
}
