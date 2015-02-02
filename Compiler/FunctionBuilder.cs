using System.Diagnostics.Contracts;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Statements;

namespace YaJS.Compiler {
	/// <summary>
	/// Анализируемая parser-ом функция
	/// </summary>
	internal sealed class FunctionBuilder {
		private readonly bool _isDeclaration;
		private readonly int _lineNo;
		private readonly string _name;
		private readonly IKeyedVariableCollection _parameterNames;
		private FunctionBodyStatement _functionBody;

		public FunctionBuilder(
			FunctionBuilder outer,
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
			DeclaredVariables = new KeyedVariableCollection();
			NestedFunctions = new KeyedFunctionCollection();
			_isDeclaration = isDeclaration;
		}

		public Function ToFunction() {
			return
				(new Function(
					_name,
					_lineNo,
					_parameterNames.ToList(),
					DeclaredVariables.ToList(),
					NestedFunctions.ToList(),
					FunctionBody,
					_isDeclaration));
		}

		public FunctionBuilder Outer { get; private set; }
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
