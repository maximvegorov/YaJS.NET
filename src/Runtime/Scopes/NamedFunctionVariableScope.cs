using System;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Scopes {
    /// <summary>
    /// Область хранения переменных, содержащая в точности одну переменную имя функции привязанную к объекту функция
    /// </summary>
    internal sealed class NamedFunctionVariableScope : VariableScope {
		public NamedFunctionVariableScope(VariableScope outerScope)
			: base(outerScope) {
			Contract.Requires(outerScope != null);
		}

	    protected override bool TryGetValue(string variableName, out JSValue value) {
	        if (Function.CompiledFunction.Name == variableName) {
	            value = Function;
	            return (true);
	        }
	        else {
	            value = null;
	            return (false);
	        }
	    }

	    public override bool ContainsVariable(string variableName) {
	        return (Function.CompiledFunction.Name == variableName);
	    }

	    protected override void DoSetVariable(string variableName, JSValue value) {
            throw new NotSupportedException();
        }

        internal void Bind(JSManagedFunction function) {
	        Contract.Requires(function != null);
	        Function = function;
	    }

        public JSManagedFunction Function { get; private set; }
	}
}
