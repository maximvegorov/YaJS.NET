using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект
	/// </summary>
	public class JSObject : JSValue, IJSObject {
		public JSObject(VirtualMachine vm, JSObject inherited = null)
			: base(JSValueType.Object) {
			Contract.Requires(vm != null);
			VM = vm;
			Inherited = inherited;
			OwnMembers = new Dictionary<string, JSValue>();
		}

		/// <summary>
		/// Виртуальная машина к которой относится объект
		/// </summary>
		protected VirtualMachine VM { get; private set; }
		/// <summary>
		/// Прототип объекта
		/// </summary>
		protected JSObject Inherited { get; private set; }
		/// <summary>
		/// Коллекция собственных свойств
		/// </summary>
		public Dictionary<string, JSValue> OwnMembers { get; private set; }
	}
}
