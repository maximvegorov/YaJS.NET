using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Компилируемая в byte-код функция
	/// </summary>
	public sealed class JSManagedFunction : JSFunction {
		internal JSManagedFunction(
			VirtualMachine vm, LocalScope outerScope, CompiledFunction compiledFunction, JSObject inherited
		)
			: base(vm, inherited) {
			Contract.Requires(compiledFunction != null);
			Contract.Requires(inherited != null);
			OuterScope = outerScope;
			CompiledFunction = compiledFunction;
		}

		/// <summary>
		/// Область хранения внешних переменных
		/// </summary>
		public LocalScope OuterScope { get; private set; }
		/// <summary>
		/// Откомпилированная функция
		/// </summary>
		public CompiledFunction CompiledFunction { get; private set; }
		/// <summary>
		/// Native-функция?
		/// </summary>
		public override bool IsNative { get { return (false); } }
	}
}
