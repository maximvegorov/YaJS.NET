using System.Diagnostics.Contracts;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime {
	/// <summary>
	/// Область хранения глобальных переменных
	/// </summary>
	public sealed class GlobalScope : VariableScope {
		private readonly JSObject _globalObject;

		public GlobalScope(JSObject globalObject)
			: base(null) {
			Contract.Requires(globalObject != null);
			_globalObject = globalObject;
		}

		public override JSValue GetVariable(string variableName) {
			return (_globalObject.GetMember(variableName));
		}

		public override void SetVariable(string variableName, JSValue value) {
			if (!_globalObject.OwnMembers.ContainsKey(variableName))
				throw new ReferenceErrorException(variableName);
			_globalObject.SetMember(variableName, value);
		}

		public override JSValue DeleteVariable(string variableName) {
			return (_globalObject.DeleteMember(variableName));
		}
	}
}
