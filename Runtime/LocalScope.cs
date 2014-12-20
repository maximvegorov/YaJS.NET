using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	using Runtime.Exceptions;

	/// <summary>
	/// Область локальных переменных
	/// </summary>
	public sealed class LocalScope {
		public LocalScope(LocalScope outerScope) {
			OuterScope = outerScope;
			Variables = new Dictionary<string, JSValue>();
		}

		internal void DeclareLocal(string variableName) {
			if (string.IsNullOrEmpty(variableName))
				throw new InvalidVariableNameException();
			Variables[variableName] = JSValue.Undefined;
		}

		internal JSValue GetLocalVariable(ExecutionThread thread, string variableName) {
			Contract.Requires(thread != null);
			if (string.IsNullOrEmpty(variableName))
				throw new InvalidVariableNameException();
			JSValue result = null;
			for (var scope = this; scope != null; scope = scope.OuterScope) {
				if (scope.Variables.TryGetValue(variableName, out result))
					return (result);
			}
			if (!thread.VM.Global.OwnMembers.TryGetValue(variableName, out result))
				throw new ReferenceErrorException(variableName);
			return (result);
		}

		internal void SetLocalVariable(ExecutionThread thread, string variableName, JSValue value) {
			Contract.Requires(thread != null);
			Contract.Requires(value != null);
			if (string.IsNullOrEmpty(variableName))
				throw new InvalidVariableNameException();
			for (var scope = this; scope != null; scope = scope.OuterScope) {
				var localVariables = scope.Variables;
				if (localVariables.ContainsKey(variableName))
					localVariables[variableName] = value;
			}
			var globalVariables = thread.VM.Global.OwnMembers;
			if (!globalVariables.ContainsKey(variableName))
				throw new ReferenceErrorException(variableName);
			globalVariables[variableName] = value;
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
