using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	[ContractClass(typeof (ContractClassForIKeyedVariableCollection))]
	internal interface IKeyedVariableCollection {
		[Pure]
		bool Contains(string variableName);

		void Add(string variableName);
		List<string> ToList();
	}

	[ContractClassFor(typeof (IKeyedVariableCollection))]
	internal abstract class ContractClassForIKeyedVariableCollection : IKeyedVariableCollection {
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

	internal sealed class EmptyKeyedVariableCollection : IKeyedVariableCollection {
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

	internal sealed class KeyedVariableCollection : IKeyedVariableCollection {
		public static readonly IKeyedVariableCollection Empty = new EmptyKeyedVariableCollection();
		private readonly List<string> _list;
		private readonly HashSet<string> _set;

		public KeyedVariableCollection() {
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
