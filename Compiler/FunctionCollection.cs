using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	using YaJS.Compiler.AST;

	public sealed class FunctionCollection {
		private List<Function> _list;
		private Dictionary<string, Function> _map;

		public FunctionCollection() {
			_list = new List<Function>();
			_map = new Dictionary<string, Function>();
		}

		[Pure]
		public bool Contains(string functionName) {
			Contract.Requires(!string.IsNullOrEmpty(functionName));
			return (_map.ContainsKey(functionName));
		}

		public void Add(Function function) {
			Contract.Requires(function != null && !(function.IsDeclaration && Contains(function.Name)));
			_list.Add(function);
			if (function.IsDeclaration) {
				_map.Add(function.Name, function);
			}
		}

		public List<Function> ToList() {
			return (_list);
		}
	}
}
