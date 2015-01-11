using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	using Runtime.Exceptions;

	/// <summary>
	/// Область локальных переменных
	/// </summary>
	public sealed class LocalScope {
		private readonly bool _isGlobal;

		internal LocalScope(Dictionary<string, JSValue> variables) {
			Contract.Requires(variables != null);
			Variables = variables;
			_isGlobal = true;
		}

		internal LocalScope(LocalScope outerScope) {
			Contract.Requires(outerScope != null);
			OuterScope = outerScope;
			Variables = new Dictionary<string, JSValue>();
		}

		internal JSValue GetVariable(string variableName) {
			if (string.IsNullOrEmpty(variableName))
				throw new InvalidVariableNameException();
			for (var scope = this; scope != null; scope = scope.OuterScope) {
				JSValue result = null;
				if (scope.Variables.TryGetValue(variableName, out result))
					return (result);
			}
			throw new ReferenceErrorException(variableName);
		}

		internal void SetVariable(string variableName, JSValue value) {
			Contract.Requires(value != null);
			if (string.IsNullOrEmpty(variableName))
				throw new InvalidVariableNameException();
			for (var scope = this; scope != null; scope = scope.OuterScope) {
				var localVariables = scope.Variables;
				if (localVariables.ContainsKey(variableName)) {
					localVariables[variableName] = value;
					return;
				}
			}
			throw new ReferenceErrorException(variableName);
		}

		internal JSValue DeleteVariable(string variableName) {
			Contract.Requires(variableName != null);
			return (_isGlobal && Variables.Remove(variableName));
		}

		/// <summary>
		/// Внешняя область локальных переменных
		/// </summary>
		public LocalScope OuterScope { get; private set; }
		/// <summary>
		/// Значения локальных переменных
		/// </summary>
		public Dictionary<string, JSValue> Variables { get; private set; }
	}
}
