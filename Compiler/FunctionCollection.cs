using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;

namespace YaJS.Compiler {
	public sealed class FunctionCollection {
		private readonly List<Function> _list;
		private readonly Dictionary<string, Function> _map;

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
			if (function.IsDeclaration)
				_map.Add(function.Name, function);
		}

		public List<Function> ToList() {
			return (_list);
		}
	}
}
