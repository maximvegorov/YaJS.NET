using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Компилируемая в byte-код функция
	/// </summary>
	public sealed class JSManagedFunction : JSFunction {
		internal JSManagedFunction(
			CompiledFunction compiledFunction, LocalScope outerScope, JSObject inherited
		) : base(inherited) {
			Contract.Requires(compiledFunction != null);
			Contract.Requires(inherited != null);
			CompiledFunction = compiledFunction;
			OuterScope = outerScope;
		}

		/// <summary>
		/// Native-функция?
		/// </summary>
		public override bool IsNative { get { return (false); } }
		/// <summary>
		/// Откомпилированная функция
		/// </summary>
		public CompiledFunction CompiledFunction { get; private set; }
		/// <summary>
		/// Область хранения внешних переменных
		/// </summary>
		public LocalScope OuterScope { get; private set; }
	}
}
