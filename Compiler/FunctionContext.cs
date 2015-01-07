using System;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	using YaJS.Compiler.AST;
	using YaJS.Compiler.AST.Statements;

	/// <summary>
	/// Контекст parsing-га функции
	/// </summary>
	internal sealed class FunctionContext {
		public FunctionContext(RootStatement rootStatement) {
			Initialize("global", VariableCollection.Empty, rootStatement);
		}

		public FunctionContext(
			string name,
			IVariableCollection parameterNames,
			RootStatement rootStatement
		) {
			Initialize(name, parameterNames, rootStatement);
		}

		public FunctionContext(
			FunctionContext outer,
			string name,
			IVariableCollection parameterNames,
			RootStatement rootStatement,
			bool isDeclaration
		) {
			Contract.Requires(outer != null);
			Outer = outer;
			Initialize(name, parameterNames, rootStatement);
			IsDeclaration = isDeclaration;
		}

		private void Initialize(string name, IVariableCollection parameterNames, RootStatement rootStatement) {
			Contract.Requires(!string.IsNullOrEmpty(Name));
			Contract.Requires(parameterNames != null);
			Contract.Requires(rootStatement != null);
			Name = name;
			DeclaredVariables = new VariableCollection();
			NestedFunctions = new FunctionCollection();
			RootStatement = rootStatement;
		}

		public Function ToFunction() {
			throw new NotImplementedException();
		}

		public FunctionContext Outer { get; private set; }
		public string Name { get; private set; }
		public IVariableCollection ParameterNames { get; private set; }
		public IVariableCollection DeclaredVariables { get; private set; }
		public FunctionCollection NestedFunctions { get; private set; }
		public RootStatement RootStatement { get; private set; }
		public bool IsDeclaration { get; private set; }
	}
}
