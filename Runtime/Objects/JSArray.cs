using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Массив
	/// </summary>
	public sealed class JSArray : JSObject {
		internal JSArray(JSObject inherited)
			: this(new List<JSValue>(), inherited) {
		}

		internal JSArray(List<JSValue> items, JSObject inherited)
			: base(inherited) {
			Contract.Requires(items != null);
			Contract.Requires(inherited != null);
			Items = items;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			for (var i = 0; i < Items.Count; i++) {
				result.Append(Items[i].ToString())
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
