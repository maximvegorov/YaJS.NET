﻿using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Псевдоператор представляющий элемент выбора в операторе switch
	/// (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	public sealed class CaseClauseStatement : LanguageStatement {
		private readonly Expression _expression;
		private StatementListStatement _statements;

		public CaseClauseStatement(int lineNo, Expression expression)
			: base(StatementType.CaseClause, lineNo) {
			Contract.Requires(expression != null);
			_expression = expression;
		}

		public override string ToString() {
			var result = new StringBuilder();
			return (result.ToString());
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("case ")
				.Append(_expression)
				.AppendLine(":");
			_statements.AppendTo(output, indent + '\t');
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			_statements.Preprocess(compiler);
		}

		public Expression Expression { get { return (_expression); } }

		public StatementListStatement Statements {
			get { return (_statements); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_statements == null);
				if (value.Parent != null)
					value.Parent.Remove();
				value.Parent = this;
				_statements = value;
			}
		}
	}
}
