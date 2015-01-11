using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Compiler {
	using AST;
	using AST.Statements;

	/// <summary>
	/// Контекст parsing-га функции
	/// </summary>
	internal sealed class FunctionContext {
		private readonly string _name;
		private readonly int _lineNo;
		private readonly IVariableCollection _parameterNames;
		private readonly List<TryStatement> _tryBlocks;
		private readonly bool _isDeclaration;

		public FunctionContext(
			FunctionContext outer,
			string name,
			int lineNo,
			IVariableCollection parameterNames,
			FunctionBody functionBody,
			bool isDeclaration
		) {
			Contract.Requires(outer != null);
			Contract.Requires(!(isDeclaration && string.IsNullOrEmpty(name)));
			Contract.Requires(parameterNames != null);
			Contract.Requires(functionBody != null);

			Outer = outer;
			_name = name;
			_lineNo = lineNo;
			_parameterNames = parameterNames;
			DeclaredVariables = new VariableCollection();
			NestedFunctions = new FunctionCollection();
			FunctionBody = functionBody;
			_tryBlocks = new List<TryStatement>();
			_isDeclaration = isDeclaration;
		}

		public void RegisterTryBlock(TryStatement tryBlock) {
			Contract.Requires(tryBlock != null);
			_tryBlocks.Add(tryBlock);
		}

		public Function ToFunction() {
			return (new Function(
				_name,
				_lineNo,
				_parameterNames.ToList(),
				DeclaredVariables.ToList(),
				NestedFunctions.ToList(),
				FunctionBody,
				_tryBlocks.Count == 0 ? Enumerable.Empty<TryStatement>() : _tryBlocks,
				_isDeclaration
			));
		}

		public FunctionContext Outer { get; private set; }
		public IVariableCollection DeclaredVariables { get; private set; }
		public FunctionCollection NestedFunctions { get; private set; }
		public FunctionBody FunctionBody { get; private set; }
	}
}
