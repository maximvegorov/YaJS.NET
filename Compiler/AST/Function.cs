using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST {
	using YaJS.Runtime;

	public sealed class Function {
		public Function(string name, List<string> formalArguments) {
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Requires(formalArguments != null);
			Name = name;
			FormalArguments = formalArguments;
			Index = -1;
			Variables = new HashSet<string>();
			Functions = new NestedFunctionCollection();
			Statements = new List<Statement>();
		}

		public CompiledFunction Compile() {
			throw new NotImplementedException();
		}

		public string Name { get; private set; }
		public List<string> FormalArguments { get; private set; }
		public int Index { get; internal set; }
		public HashSet<string> Variables { get; private set; }
		public NestedFunctionCollection Functions { get; private set; }
		public List<Statement> Statements { get; private set; }
	}
}
