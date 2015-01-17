using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	[ContractClass(typeof (ContractClassForIVariableCollection))]
	internal interface IVariableCollection {
		[Pure]
		bool Contains(string variableName);

		void Add(string variableName);
		List<string> ToList();
	}

	[ContractClassFor(typeof (IVariableCollection))]
	internal abstract class ContractClassForIVariableCollection : IVariableCollection {
		public bool Contains(string variableName) {
			Contract.Requires(!string.IsNullOrEmpty(variableName));
			throw new NotImplementedException();
		}

		public void Add(string variableName) {
			Contract.Requires(!Contains(variableName));
			throw new NotImplementedException();
		}

		public List<string> ToList() {
			throw new NotImplementedException();
		}
	}

	internal sealed class EmptyVariableCollection : IVariableCollection {
		public bool Contains(string variableName) {
			return (false);
		}

		public void Add(string variableName) {
			throw new NotSupportedException();
		}

		public List<string> ToList() {
			return (new List<string>());
		}
	}

	internal sealed class VariableCollection : IVariableCollection {
		public static readonly IVariableCollection Empty = new EmptyVariableCollection();
		private readonly List<string> _list;
		private readonly HashSet<string> _set;

		public VariableCollection() {
			_list = new List<string>();
			_set = new HashSet<string>();
		}

		public bool Contains(string variableName) {
			return (_set.Contains(variableName));
		}

		public void Add(string variableName) {
			_list.Add(variableName);
			_set.Add(variableName);
		}

		public List<string> ToList() {
			return (_list);
		}
	}
}
