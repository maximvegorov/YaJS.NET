using System;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Интерфейс будет использоваться для расширения глобального объекта
	/// виртуальной машины расширенными объектами и функциями
	/// </summary>
	[ContractClass(typeof(ContractClassForIJSModuleInitializer))]
	public interface IJSModuleInitializer {
		void ApplyTo(VirtualMachine vm);
	}

	[ContractClassFor(typeof(IJSModuleInitializer))]
	internal abstract class ContractClassForIJSModuleInitializer : IJSModuleInitializer {
		public void ApplyTo(VirtualMachine vm) {
			Contract.Requires<ArgumentNullException>(vm != null);
		}
	}
}
