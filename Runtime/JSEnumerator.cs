using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Перечислитель. Используется для реализации for-in
	/// </summary>
	internal sealed class JSEnumerator : JSValue {
		private readonly IEnumerator<JSValue> _enumerator;

		public JSEnumerator(IEnumerator<JSValue> enumerator)
			: base(JSValueType.Enumerator) {
			Contract.Requires(enumerator != null);
			_enumerator = enumerator;
		}

		public bool MoveNext() {
			return (_enumerator.MoveNext());
		}

		internal override JSEnumerator RequireEnumerator() {
			return (this);
		}

		public JSValue Current {
			get { return (_enumerator.Current); }
		}
	}
}
