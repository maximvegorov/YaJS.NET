using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Блок try
	/// </summary>
	internal sealed class TryBlockInfo {
		public TryBlockInfo(int handlerOffset, LocalScope scope, TryBlockInfo outerBlock) {
			Contract.Requires(handlerOffset > 0);
			Contract.Requires(scope != null);
			HandlerOffset = handlerOffset;
			Scope = scope;
			OuterBlock = outerBlock;
		}

		/// <summary>
		/// Внешний блок try в выполняемой функции
		/// </summary>
		public TryBlockInfo OuterBlock { get; private set; }

		/// <summary>
		/// Смещение в коде блока catch/finally
		/// </summary>
		public int HandlerOffset { get; private set; }

		/// <summary>
		/// Область локальных переменных на момент входа в блок try
		/// </summary>
		public LocalScope Scope { get; private set; }
	}
}
