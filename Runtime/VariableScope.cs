using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Область видимости переменных
	/// </summary>
	public abstract class VariableScope {
		protected VariableScope(VariableScope outerScope) {
			OuterScope = outerScope;
		}

		public virtual JSValue GetVariable(string variableName) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			return (null);
		}

		public virtual void SetVariable(string variableName, JSValue value) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			Contract.Requires(value != null);
		}

		public virtual JSValue DeleteVariable(string variableName) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			return (false);
		}

		public VariableScope OuterScope { get; private set; }
	}
}
