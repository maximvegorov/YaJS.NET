using System.Diagnostics.Contracts;
using YaJS.Runtime.Exceptions;

namespace YaJS.Runtime {
	/// <summary>
	/// Область видимости переменных
	/// </summary>
	public abstract class VariableScope {
		protected VariableScope(VariableScope outerScope) {
			OuterScope = outerScope;
		}

		public virtual void DeclareVariable(string variableName, JSValue initialValue) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(initialValue != null);
		}

		public virtual void DeclareVariableIfNotExists(string variableName, JSValue initialValue) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(initialValue != null);
		}

		protected abstract bool TryGetValue(string variableName, out JSValue value);

		public virtual JSValue GetVariable(string variableName) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			for (var scope = this; scope != null; scope = scope.OuterScope) {
				JSValue result = null;
				if (scope.TryGetValue(variableName, out result))
					return (result);
			}
			throw new ReferenceErrorException(variableName);
		}

		public abstract bool ContainsVariable(string variableName);
		protected abstract void DoSetVariable(string variableName, JSValue value);

		public void SetVariable(string variableName, JSValue value) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(value != null);
			for (var scope = this; scope != null; scope = scope.OuterScope) {
				if (ContainsVariable(variableName)) {
					DoSetVariable(variableName, value);
					return;
				}
			}
			throw new ReferenceErrorException(variableName);
		}

		/// <summary>
		/// Внешняя область видимости переменных
		/// </summary>
		public VariableScope OuterScope { get; private set; }
	}
}
