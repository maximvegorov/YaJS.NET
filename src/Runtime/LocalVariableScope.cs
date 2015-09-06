using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Область хранения локальных переменных
	/// </summary>
	public sealed class LocalVariableScope : VariableScope {
		private readonly Dictionary<string, JSValue> _variables;

		internal LocalVariableScope(VariableScope outerScope)
			: base(outerScope) {
			Contract.Requires(outerScope != null);
			_variables = new Dictionary<string, JSValue>();
		}

		public override void DeclareVariable(string variableName, JSValue initialValue) {
			_variables.Add(variableName, initialValue);
		}

		public override void DeclareVariableIfNotExists(string variableName, JSValue initialValue) {
			if (!_variables.ContainsKey(variableName))
				_variables.Add(variableName, initialValue);
		}

		protected override bool TryGetValue(string variableName, out JSValue value) {
			return (_variables.TryGetValue(variableName, out value));
		}

		public override bool ContainsVariable(string variableName) {
			return (_variables.ContainsKey(variableName));
		}

		protected override void DoSetVariable(string variableName, JSValue value) {
			_variables[variableName] = value;
		}
	}
}
