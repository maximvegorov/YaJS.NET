using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Statements;

namespace YaJS.Compiler {
	/// <summary>
	/// Анализируемая parser-ом функция
	/// </summary>
	internal sealed class ParsingFunction {
		private readonly bool _isDeclaration;
		private readonly int _lineNo;
		private readonly string _name;
		private readonly IKeyedVariableCollection _parameterNames;
		private readonly HashSet<string> _directives;
		private FunctionBodyStatement _functionBody;

		public ParsingFunction(
			ParsingFunction outer,
			string name,
			int lineNo,
			IKeyedVariableCollection parameterNames,
			bool isDeclaration) {
			Contract.Requires(!(isDeclaration && string.IsNullOrEmpty(name)));
			Contract.Requires(parameterNames != null);

			Outer = outer;
			_name = name;
			_lineNo = lineNo;
			_parameterNames = parameterNames;
			_directives = new HashSet<string>();
			DeclaredVariables = new KeyedVariableCollection();
			NestedFunctions = new KeyedFunctionCollection();
			_isDeclaration = isDeclaration;
		}

		public void AppendDirective(string directive) {
			Contract.Requires(!string.IsNullOrEmpty(directive));
			_directives.Add(directive);
		}

		public Function ToFunction() {
			return
				(new Function(
					_name,
					_lineNo,
					_parameterNames.ToList(),
					_directives,
					DeclaredVariables.ToList(),
					NestedFunctions.ToList(),
					FunctionBody,
					_isDeclaration));
		}

		public ParsingFunction Outer { get; private set; }
		public IKeyedVariableCollection DeclaredVariables { get; private set; }
		public KeyedFunctionCollection NestedFunctions { get; private set; }

		public FunctionBodyStatement FunctionBody {
			get { return (_functionBody); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_functionBody == null);
				_functionBody = value;
			}
		}
	}
}
