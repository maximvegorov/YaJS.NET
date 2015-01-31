using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime {
	/// <summary>
	/// Область хранения глобальных переменных
	/// </summary>
	public sealed class GlobalVariableScope : VariableScope {
		private readonly JSObject _globalObject;

		public GlobalVariableScope(JSObject globalObject)
			: base(null) {
			Contract.Requires(globalObject != null);
			_globalObject = globalObject;
		}

		public override void DeclareVariable(string variableName, JSValue initialValue) {
			_globalObject.OwnMembers.Add(variableName, initialValue);
		}

		public override void DeclareVariableIfNotExists(string variableName, JSValue initialValue) {
			if (!_globalObject.OwnMembers.ContainsKey(variableName))
				_globalObject.OwnMembers.Add(variableName, initialValue);
		}

		protected override bool TryGetValue(string variableName, out JSValue value) {
			value = _globalObject.GetMember(variableName);
			return (value != null);
		}

		public override bool ContainsVariable(string variableName) {
			return (_globalObject.OwnMembers.ContainsKey(variableName));
		}

		protected override void DoSetVariable(string variableName, JSValue value) {
			_globalObject.OwnMembers[variableName] = value;
		}
	}
}
