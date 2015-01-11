using System;

namespace YaJS.Runtime.Enumerators {
	/// <summary>
	/// Пустой перечислитель
	/// </summary>
	internal sealed class JSEmptyEnumerator : JSEnumerator {
		public override bool MoveNext() {
			return (false);
		}
		public override JSValue Current { get { throw new NotSupportedException(); } }
	}
}
