using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	/// <summary>
	/// Таблица переходов для инструкции Switch
	/// </summary>
	[Serializable]
	public sealed class SwitchJumpTable {
		private readonly int _defaultJump;
		private readonly Dictionary<JSValue, int> _jumps;

		public SwitchJumpTable(Dictionary<JSValue, int> jumps, int defaultJump) {
			Contract.Requires(jumps != null);
			Contract.Requires(defaultJump >= 0);
			_jumps = jumps;
			_defaultJump = defaultJump;
		}

		public int Jump(JSValue selector) {
			Contract.Requires(selector != null);
			int offset;
			if (!_jumps.TryGetValue(selector, out offset))
				offset = _defaultJump;
			return (offset);
		}
	}
}
