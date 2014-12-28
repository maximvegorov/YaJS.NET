using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	public sealed class NestedFunctionCollection {
		private List<Function> list;
		private Dictionary<string, Function> map;
		private int count;

		public NestedFunctionCollection() {
			list = new List<Function>();
			map = new Dictionary<string, Function>();
		}

		public void Add(Function function) {
			Contract.Requires(function != null);
			function.Index = list.Count;
			list.Add(function);
			map.Add(function.Name, function);
			count++;
		}

		public bool Contains(string name) {
			Contract.Requires(!string.IsNullOrEmpty(name));
			return (map.ContainsKey(name));
		}

		public bool TryGet(string name, out Function function) {
			Contract.Requires(!string.IsNullOrEmpty(name));
			return (map.TryGetValue(name, out function));
		}

		public Function this[int index] {
			get {
				Contract.Requires(0 <= index && index < Count);
				return (list[index]);
			}
			set {
				Contract.Requires(0 <= index && index < Count);
				list[index] = value;
			}
		}
		public Function this[string name] {
			get {
				Contract.Requires(!string.IsNullOrEmpty(name));
				return (map[name]);
			}
			set {
				Contract.Requires(!string.IsNullOrEmpty(name));
				map[name] = value;
			}
		}
		public int Count { get { return (count); } }
	}
}
