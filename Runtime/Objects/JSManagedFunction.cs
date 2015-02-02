using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Компилируемая в byte-код функция
	/// </summary>
	public sealed class JSManagedFunction : JSFunction {
		internal JSManagedFunction(
			VirtualMachine vm,
			VariableScope outerScope,
			CompiledFunction compiledFunction,
			JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(compiledFunction != null);
			Contract.Requires(inherited != null);
			OuterScope = outerScope;
			CompiledFunction = compiledFunction;
		}

		public override string ToString() {
			return ("Managed function");
		}

		/// <summary>
		/// Область хранения внешних переменных
		/// </summary>
		public VariableScope OuterScope { get; private set; }

		/// <summary>
		/// Откомпилированная функция
		/// </summary>
		public CompiledFunction CompiledFunction { get; private set; }
		public override bool IsNative { get { return (false); } }
		public override int ParameterCount { get { return (CompiledFunction.ParameterNames.Length); } }
	}
}
