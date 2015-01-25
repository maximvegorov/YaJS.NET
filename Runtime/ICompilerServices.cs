using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Сервис компилятора. Используется для компиляции JS кода в byte-код на лету
	/// </summary>
	[ContractClass(typeof (ContractClassForICompilerServices))]
	public interface ICompilerServices {
		CompiledFunction Compile(string functionName, IEnumerable<string> parameterNames, string functionBody);
	}

	[ContractClassFor(typeof (ICompilerServices))]
	internal abstract class ContractClassForICompilerServices : ICompilerServices {
		public CompiledFunction Compile(string functionName, IEnumerable<string> parameterNames, string functionBody) {
			Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(functionName), "functionName");
			Contract.Requires<ArgumentNullException>(parameterNames != null, "parameterNames");
			Contract.Requires<ArgumentNullException>(functionBody != null, "functionBody");
			throw new NotImplementedException();
		}
	}
}
