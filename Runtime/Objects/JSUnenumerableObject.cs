using System.Collections.Generic;
using System.Linq;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект с неперечисляемыми свойствами
	/// </summary>
	public class JSUnenumerableObject : JSObject {
		public JSUnenumerableObject(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		protected override IEnumerable<string> GetEnumerableMembers() {
			return (Enumerable.Empty<string>());
		}
	}
}
