using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaJS.Runtime {
	/// <summary>
	/// Область хранения переменнах содержащая в точности одну переменную
	/// </summary>
	public sealed class SingletonVariableScope : VariableScope {
		internal SingletonVariableScope(VariableScope outerScope)
			: base(outerScope) {
			Contract.Requires(outerScope != null);
		}

		internal 
	}
}
