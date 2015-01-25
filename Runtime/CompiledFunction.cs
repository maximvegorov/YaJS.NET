using System;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Откомпилированная функция
	/// </summary>
	[Serializable]
	public sealed class CompiledFunction {
		public static readonly string[] EmptyParameterNames = new string[0];
		public static readonly string[] EmptyDeclaredVariables = new string[0];
		public static readonly CompiledFunction[] EmptyNestedFunctions = new CompiledFunction[0];
		public static readonly SwitchJumpTable[] EmptySwitchJumpTables = new SwitchJumpTable[0];

		public CompiledFunction(
			string name,
			int lineNo,
			string[] parameterNames,
			string[] declaredVariables,
			CompiledFunction[] nestedFunctions,
			int declaredFunctionCount,
			string source,
			byte[] compiledCode,
			SwitchJumpTable[] switchJumpTables) {
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Requires(lineNo >= 1);
			Contract.Requires(parameterNames != null);
			Contract.Requires(declaredVariables != null);
			Contract.Requires(nestedFunctions != null);
			Contract.Requires(0 <= declaredFunctionCount && declaredFunctionCount < nestedFunctions.Length);
			Contract.Requires(!string.IsNullOrEmpty(source));
			Contract.Requires(compiledCode != null && compiledCode.Length > 0);
			Contract.Requires(switchJumpTables != null);

			Name = name;
			LineNo = lineNo;
			ParameterNames = parameterNames;
			DeclaredVariables = declaredVariables;
			NestedFunctions = nestedFunctions;
			DeclaredFunctionCount = declaredFunctionCount;
			Source = source;
			CompiledCode = compiledCode;
			SwitchJumpTables = switchJumpTables;
		}

		/// <summary>
		/// Имя функции
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Номер строки на которой начинается функция
		/// </summary>
		public int LineNo { get; private set; }

		/// <summary>
		/// Параметры
		/// </summary>
		public string[] ParameterNames { get; private set; }

		/// <summary>
		/// Объявленные переменные
		/// </summary>
		public string[] DeclaredVariables { get; private set; }

		/// <summary>
		/// Вложенные функции
		/// </summary>
		public CompiledFunction[] NestedFunctions { get; private set; }

		/// <summary>
		/// Количество FunctionDeclaration
		/// </summary>
		public int DeclaredFunctionCount { get; private set; }

		/// <summary>
		/// Таблицы переходов для оператора switch
		/// </summary>
		public SwitchJumpTable[] SwitchJumpTables { get; private set; }

		/// <summary>
		/// Исходный код функции
		/// </summary>
		public string Source { get; private set; }

		/// <summary>
		/// Byte-код функции
		/// </summary>
		public byte[] CompiledCode { get; private set; }
	}
}
