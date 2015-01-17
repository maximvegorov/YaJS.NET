using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Набор меток оператора
	/// </summary>
	[ContractClass(typeof (ContractClassForILabelSet))]
	public interface ILabelSet {
		[Pure]
		bool Contains(string label);

		ILabelSet UnionWith(string label);
	}

	[ContractClassFor(typeof (ILabelSet))]
	internal abstract class ContractClassForILabelSet : ILabelSet {
		public bool Contains(string label) {
			Contract.Requires(label != null);
			throw new NotImplementedException();
		}

		public ILabelSet UnionWith(string label) {
			Contract.Requires(!Contains(label));
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Пустой набор меток
	/// </summary>
	internal sealed class EmptyLabelSet : ILabelSet {
		private static readonly ILabelSet OneEmptyStringLabelSet = new SingletonLabelSet(string.Empty);

		public bool Contains(string label) {
			return (false);
		}

		public ILabelSet UnionWith(string label) {
			if (string.IsNullOrEmpty(label))
				return (OneEmptyStringLabelSet);
			return (new SingletonLabelSet(label));
		}
	}

	/// <summary>
	/// Набор меток содержащий одну метку
	/// </summary>
	internal sealed class SingletonLabelSet : ILabelSet {
		private readonly string _label;

		public SingletonLabelSet(string label) {
			Contract.Requires(label != null);
			_label = label;
		}

		[Pure]
		public bool Contains(string label) {
			return (string.CompareOrdinal(_label, label) == 0);
		}

		public ILabelSet UnionWith(string label) {
			return (new TwoLabelSet(_label, label));
		}
	}

	/// <summary>
	/// Набор меток содержащий две метки
	/// </summary>
	internal sealed class TwoLabelSet : ILabelSet {
		private readonly string _label1;
		private readonly string _label2;

		public TwoLabelSet(string label1, string label2) {
			Contract.Requires(label1 != null);
			Contract.Requires(label2 != null);
			_label1 = label1;
			_label2 = label2;
		}

		[Pure]
		public bool Contains(string label) {
			return (string.CompareOrdinal(_label1, label) == 0 || string.CompareOrdinal(_label2, label) == 0);
		}

		public ILabelSet UnionWith(string label) {
			return (new LabelSet(_label1, _label2, label));
		}
	}

	/// <summary>
	/// Набор содержащий произвольное кол-во меток
	/// </summary>
	internal sealed class LabelSet : ILabelSet {
		private readonly HashSet<string> _set;

		public LabelSet(string label1, string label2, string label3) {
			Contract.Requires(label1 != null);
			Contract.Requires(label2 != null);
			Contract.Requires(label3 != null);
			_set = new HashSet<string> {label1, label2, label3};
		}

		[Pure]
		public bool Contains(string label) {
			return (_set.Contains(label));
		}

		public ILabelSet UnionWith(string label) {
			_set.Add(label);
			return (this);
		}
	}
}
