using System;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.Emitter {
	/// <summary>
	/// Используется при генерации операторов goto
	/// </summary>
	internal sealed class Label {
		private static readonly IUnresolvedOffsetSet EmptyUnresolvedOffsets = new EmptyUnresolvedOffsetSet();
		private readonly ByteCodeEmitter _emitter;
		private IUnresolvedOffsetSet _unresolvedOffsets;

		public Label(ByteCodeEmitter emitter) {
			Contract.Requires(emitter != null);
			_emitter = emitter;
			_unresolvedOffsets = EmptyUnresolvedOffsets;
		}

		public void AppendUnresolved(int offset) {
			_unresolvedOffsets = _unresolvedOffsets.Append(offset);
		}

		public void Resolve(int offset) {
			Contract.Requires(!Offset.HasValue);
			Offset = offset;
			var offsetAsBytes = BitConverter.GetBytes(offset);
			foreach (var unresolvedOffset in _unresolvedOffsets.Offsets)
				Buffer.BlockCopy(offsetAsBytes, 0, _emitter.RawBuffer, unresolvedOffset, sizeof (int));
			_unresolvedOffsets = EmptyUnresolvedOffsets;
		}

		/// <summary>
		/// Смещение в байт коде
		/// </summary>
		public int? Offset { get; private set; }
	}
}
