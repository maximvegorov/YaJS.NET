using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Сервис компилятора. Используется для компиляции JS кода в byte-код на лету
	/// </summary>
	[ContractClass(typeof (ContractClassForICompilerServices))]
	public interface ICompilerServices {
		/// <summary>
		/// Компилирует JS в byte-код
		/// </summary>
		/// <param name="functionName">Имя функции</param>
		/// <param name="parameterNames">Список параметров</param>
		/// <param name="functionBody">Исходный код функции</param>
		/// <param name="isEvalMode">
		/// Должна ли функции компилироваться в режиме eval. В режиме eval результатом функции является результат
		/// последнего ExpressionStatement, если нет явного ReturnStatement
		/// </param>
		/// <returns>Откомпилированную функцию</returns>
		CompiledFunction Compile(
			string functionName, IEnumerable<string> parameterNames, string functionBody, bool isEvalMode);
	}

	[ContractClassFor(typeof (ICompilerServices))]
	internal abstract class ContractClassForICompilerServices : ICompilerServices {
		public CompiledFunction Compile(
			string functionName, IEnumerable<string> parameterNames, string functionBody, bool isEvalMode) {
			Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(functionName), "functionName");
			Contract.Requires<ArgumentNullException>(parameterNames != null, "parameterNames");
			Contract.Requires<ArgumentNullException>(functionBody != null, "functionBody");
			throw new NotImplementedException();
		}
	}
}
