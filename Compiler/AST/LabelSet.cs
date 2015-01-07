using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Набор меток оператора
	/// </summary>
	public interface ILabelSet {
		bool Contains(string label);
		ILabelSet UnionWith(string label);
	}

	/// <summary>
	/// Пустой набор меток
	/// </summary>
	internal sealed class EmptyLabelSet : ILabelSet {
		public bool Contains(string label) {
			Contract.Requires(label != null);
			return (false);
		}

		public ILabelSet UnionWith(string label) {
			Contract.Requires(label != null);
			return (new SingletonLabelSet(label));
		}
	}

	/// <summary>
	/// Набор меток содержащий одну метку
	/// </summary>
	internal sealed class SingletonLabelSet : ILabelSet {
		private string _label;

		public SingletonLabelSet(string label) {
			Contract.Requires(label != null);
			this._label = label;
		}

		[Pure]
		public bool Contains(string label) {
			Contract.Requires(label != null);
			return (string.CompareOrdinal(_label, label) == 0);
		}

		public ILabelSet UnionWith(string label) {
			Contract.Requires(!Contains(label));
			return (new TwoLabelSet(_label, label));
		}
	}

	/// <summary>
	/// Набор меток содержащий две метки
	/// </summary>
	internal sealed class TwoLabelSet : ILabelSet {
		private string _label1;
		private string _label2;

		public TwoLabelSet(string label1, string label2) {
			Contract.Requires(label1 != null);
			Contract.Requires(label2 != null);
			_label1 = label1;
			_label2 = label2;
		}

		[Pure]
		public bool Contains(string label) {
			Contract.Requires(label != null);
			return (string.CompareOrdinal(_label1, label) == 0 || string.CompareOrdinal(_label2, label) == 0);
		}

		public ILabelSet UnionWith(string label) {
			Contract.Requires(!Contains(label));
			return (new HashLabelSet(_label1, _label2, label));
		}
	}

	/// <summary>
	/// Набор содержащий произвольное кол-во меток
	/// </summary>
	internal sealed class HashLabelSet : ILabelSet {
		private HashSet<string> _set;

		public HashLabelSet(string label1, string label2, string label3) {
			Contract.Requires(label1 != null);
			Contract.Requires(label2 != null);
			Contract.Requires(label3 != null);
			_set = new HashSet<string>() { label1, label2, label3 };
		}

		[Pure]
		public bool Contains(string label) {
			Contract.Requires(label != null);
			return (_set.Contains(label));
		}

		public ILabelSet UnionWith(string label) {
			Contract.Requires(!Contains(label));
			_set.Add(label);
			return (this);
		}
	}
}
