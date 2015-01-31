using System.Collections.Generic;
using System.Linq;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Прототип с неперечисляемыми свойствами
	/// </summary>
	public class JSUnenumerablePrototype : JSObject {
		public JSUnenumerablePrototype(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		protected override IEnumerable<string> GetEnumerableMembers() {
			return (Enumerable.Empty<string>());
		}
	}
}
