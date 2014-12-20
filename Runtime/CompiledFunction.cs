using System;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Откомпилированная функция
	/// </summary>
	[Serializable]
	public sealed class CompiledFunction {
		public CompiledFunction(
			string name,
			string[] arguments,
			CompiledFunction[] nestedFunctions,
			string source,
			byte[] compiledCode
		) {
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Requires(arguments != null);
			Contract.Requires(nestedFunctions != null);
			Contract.Requires(!string.IsNullOrEmpty(source));
			Contract.Requires(compiledCode != null && compiledCode.Length > 0);

			Name = name;
			Arguments = arguments;
			NestedFunctions = nestedFunctions;
			Source = source;
			CompiledCode = compiledCode;
		}

		/// <summary>
		/// Имя функции
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Параметры
		/// </summary>
		public string[] Arguments { get; private set; }
		/// <summary>
		/// Вложенные функции
		/// </summary>
		public CompiledFunction[] NestedFunctions { get; private set; }
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
