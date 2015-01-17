using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Базовый класс для всех ошибок
	/// </summary>
	public class JSError : JSObject {
		public JSError(VirtualMachine vm, string message, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(inherited != null);
			OwnMembers.Add("name", "Error");
			OwnMembers.Add("message", message);
		}
	}
}
