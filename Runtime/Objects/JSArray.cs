using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Массив
	/// </summary>
	internal sealed class JSArray : JSObject {
		public JSArray(VirtualMachine vm, JSObject inherited)
			: this(vm, new List<JSValue>(), inherited) {
		}

		public JSArray(VirtualMachine vm, List<JSValue> items, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(items != null);
			Contract.Requires(inherited != null);
			Items = items;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			for (var i = 0; i < Items.Count; i++) {
				result.Append(Items[i])
					.Append(", ");
			}
			if (result.Length > 1)
				result.Length -= 2;
			result.Append(']');
			return (result.ToString());
		}

		/// <summary>
		/// Элементы массива
		/// </summary>
		public List<JSValue> Items { get; private set; }
	}
}
