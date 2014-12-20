using System;
using System.Collections.Generic;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект
	/// </summary>
	public class JSObject : JSValue {
		public JSObject(JSObject inherited = null)
			: base(JSValueType.Object) {
			Inherited = inherited;
			OwnMembers = new Dictionary<string, JSValue>();
		}

		/// <summary>
		/// Прототип объекта
		/// </summary>
		public JSObject Inherited { get; private set; }
		/// <summary>
		/// Коллекция собственных свойств
		/// </summary>
		public Dictionary<string, JSValue> OwnMembers { get; private set; }
	}
}
