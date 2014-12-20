using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Базовый класс для всех ошибок
	/// </summary>
	public class JSError : JSObject {
		public JSError(string message, JSObject inherited) : base(inherited) {
			Contract.Requires(inherited != null);
			OwnMembers.Add("name", JSValue.Create("Error"));
			OwnMembers.Add("message", JSValue.Create(message));
		}
	}
}
