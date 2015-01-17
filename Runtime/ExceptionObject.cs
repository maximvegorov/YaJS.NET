using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Объект wrapper для JS исключений
	/// </summary>
	public sealed class ExceptionObject {
		public ExceptionObject(JSValue thrownValue, ExceptionObject inner) {
			Contract.Requires(thrownValue != null);
			ThrownValue = thrownValue;
			Inner = inner;
		}

		public JSValue ThrownValue { get; private set; }
		public ExceptionObject Inner { get; private set; }
	}
}
