using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST.Statements;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Представляет в AST дереве функцию
	/// </summary>
	public sealed class Function {
		public Function(
			string name,
			int lineNo,
			List<string> parameterNames,
			List<string> declaredVariables,
			List<Function> nestedFunctions,
			Statement functionBody,
			IEnumerable<TryStatement> tryStatements,
			bool isDeclaration
			) {
			Contract.Requires(!(isDeclaration && string.IsNullOrEmpty(name)));
			Contract.Requires(parameterNames != null);
			Contract.Requires(declaredVariables != null);
			Contract.Requires(nestedFunctions != null);
			Contract.Requires(functionBody != null);
			Contract.Requires(tryStatements != null);

			Name = name;
			LineNo = lineNo;
			ParameterNames = parameterNames;
			DeclaredVariables = declaredVariables;
			NestedFunctions = nestedFunctions;
			FunctionBody = functionBody;
			TryStatements = tryStatements;
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
		/// Копирует операторы блока finally перед каждой точкой выхода из блока try
		/// </summary>
		internal void ProcessTryFinallyStatements() {
			foreach (var tryStatement in TryStatements) {
				if (tryStatement.FinallyBlock == null || tryStatement.TryBlock.ExitPoints.Count == 0)
					continue;
				var finallyBlock = tryStatement.FinallyBlock;
				foreach (var exitPoint in tryStatement.TryBlock.ExitPoints) {
					exitPoint.InsertBefore(new ReferenceStatement(null, finallyBlock));
				}
			}
		}

		/// <summary>
		/// Имя функции. Обязательно для FunctionDeclaration
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Номер строки на которой начинается функция
		/// </summary>
		public int LineNo { get; private set; }

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
		/// Операторы try содержащиеся в функции
		/// </summary>
		public IEnumerable<TryStatement> TryStatements { get; private set; }

		/// <summary>
		/// Является ли функция FunctionDeclaration
		/// </summary>
		public bool IsDeclaration { get; private set; }

		/// <summary>
		/// Кол-во FunctionDeclaration в списке NestedFunctions
		/// </summary>
		public int FunctionDeclarationCount { get; private set; }

		/// <summary>
		/// Индекс функции в списке вложенных функций внешней функции (используется для кодогенерации)
		/// </summary>
		public int Index { get; private set; }
	}
}
