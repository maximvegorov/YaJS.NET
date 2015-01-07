using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Представляет в AST дереве функцию
	/// </summary>
	public sealed class Function {
		public Function(string name, List<string> formalArguments, bool isDeclaration) {
			Contract.Requires(isDeclaration && !string.IsNullOrEmpty(name));
			Contract.Requires(formalArguments != null);
			Name = name;
			FormalArguments = formalArguments;
			IsDeclaration = isDeclaration;
			DeclaredVariables = new List<string>();
			NestedFunctions = new List<Function>();
			FunctionBody = new List<Statement>();
		}

		/// <summary>
		/// Имя функции. Обязательно для FunctionDeclaration
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Список имен параметров функции
		/// </summary>
		public List<string> FormalArguments { get; private set; }
		/// <summary>
		/// Является ли функция FunctionDeclaration
		/// </summary>
		public bool IsDeclaration { get; set; }
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
		public List<Statement> FunctionBody { get; private set; }
		/// <summary>
		/// Индекс функции в списке вложенных функций внешней функции
		/// </summary>
		public int Index { get; internal set; }
	}
}
