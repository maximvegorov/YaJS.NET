using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Compiler.Emitter {
	/// <summary>
	/// Набор неразрешенных смещений в операторах goto
	/// </summary>
	internal interface IUnresolvedOffsetSet {
		IUnresolvedOffsetSet Append(int offset);
		IEnumerable<int> Offsets { get; }
	}

	/// <summary>
	/// Пустой набор
	/// </summary>
	internal sealed class EmptyUnresolvedOffsetSet : IUnresolvedOffsetSet {
		public IUnresolvedOffsetSet Append(int offset) {
			return (new SingletonUnresolvedOffsetSet(offset));
		}

		public IEnumerable<int> Offsets { get { return (Enumerable.Empty<int>()); } }
	}

	/// <summary>
	/// Набор содержащий одно неразрешенное смещение
	/// </summary>
	internal sealed class SingletonUnresolvedOffsetSet : IUnresolvedOffsetSet {
		private readonly int _offset;

		public SingletonUnresolvedOffsetSet(int offset) {
			Contract.Requires(offset >= 0);
			_offset = offset;
		}

		public IUnresolvedOffsetSet Append(int offset) {
			return (new TwoUnresolvedOffsetSet(_offset, offset));
		}

		public IEnumerable<int> Offsets { get { yield return _offset; } }
	}

	/// <summary>
	/// Набор содержащий два неразрешенных смещения
	/// </summary>
	internal sealed class TwoUnresolvedOffsetSet : IUnresolvedOffsetSet {
		private readonly int _offset1;
		private readonly int _offset2;

		public TwoUnresolvedOffsetSet(int offset1, int offset2) {
			Contract.Requires(offset1 >= 0);
			Contract.Requires(offset2 >= 0);
			_offset1 = offset1;
			_offset2 = offset2;
		}

		public IUnresolvedOffsetSet Append(int offset) {
			return (new UnresolvedOffsetSet(_offset1, _offset2, offset));
		}

		public IEnumerable<int> Offsets { get { return (new[] {_offset1, _offset2}); } }
	}

	/// <summary>
	/// Набор содержащий произвольное кол-во неразрешенных смещений
	/// </summary>
	internal sealed class UnresolvedOffsetSet : IUnresolvedOffsetSet {
		private readonly List<int> _list;

		public UnresolvedOffsetSet(int offset1, int offset2, int offset3) {
			Contract.Requires(offset1 >= 0);
			Contract.Requires(offset2 >= 0);
			Contract.Requires(offset3 >= 0);
			_list = new List<int> {offset1, offset2, offset3};
		}

		public IUnresolvedOffsetSet Append(int offset) {
			_list.Add(offset);
			return (this);
		}

		public IEnumerable<int> Offsets { get { return (_list); } }
	}
}
