using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор switch (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.11)
	/// </summary>
	public sealed class SwitchStatement : LabellableStatement {
		private Expression _expression;
		private CaseClauseBlockStatement _beforeDefault;
		private StatementListStatement _defaultClause;
		private CaseClauseBlockStatement _afterDefault;

		public SwitchStatement(int lineNo, ILabelSet labelSet)
			: base(StatementType.Switch, lineNo, labelSet) {
		}

		protected internal override void AppendTo(StringBuilder output, string indent) {
			output.Append(indent)
				.Append("switch (")
				.Append(_expression)
				.AppendLine(") {");
			_beforeDefault.AppendTo(output, indent);
			if (_defaultClause != null) {
				output.Append(indent)
					.AppendLine("default:");
				_defaultClause.AppendTo(output, indent + '\t');
				if (_afterDefault != null)
					_afterDefault.AppendTo(output, indent);
			}
			output.Append(indent)
				.AppendLine("}");
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			if (_beforeDefault == null)
				Errors.ThrowInternalError();
			else
				_beforeDefault.Preprocess(compiler);
			if (_defaultClause != null)
				_defaultClause.Preprocess(compiler);
			if (_afterDefault != null)
				_afterDefault.Preprocess(compiler);
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			var endLabel = compiler.Emitter.DefineLabel();
			compiler.StatementEnds.Add(this, endLabel);
			try {
				var jumps = new Dictionary<JSValue, int>();
				foreach (var caseClause in _beforeDefault) {
					var offset = compiler.Emitter.Offset;
					caseClause.Statements.CompileBy(compiler);
					jumps.Add(caseClause.Expression.ToJSValue(), offset);
				}
				int? defaultOffset = null;
				if (_defaultClause != null) {
					defaultOffset = compiler.Emitter.Offset;
					_defaultClause.CompileBy(compiler);
				}
				foreach (var caseClause in _afterDefault) {
					var offset = compiler.Emitter.Offset;
					caseClause.Statements.CompileBy(compiler);
					jumps.Add(caseClause.Expression.ToJSValue(), offset);
				}
				compiler.Emitter.MarkLabel(endLabel);
				Contract.Assert(endLabel.Offset.HasValue);
				if (!defaultOffset.HasValue)
					defaultOffset = endLabel.Offset.Value;
				Contract.Assert(defaultOffset.HasValue);
				compiler.SwitchJumpTables.Add(new SwitchJumpTable(jumps, defaultOffset.Value));
			}
			finally {
				compiler.StatementEnds.Remove(this);
			}
			compiler.MarkEndOfStatement();
		}

		public Expression Expression {
			set {
				Contract.Requires(value != null);
				Contract.Assert(_expression == null);
				_expression = value;
			}
		}

		public CaseClauseBlockStatement BeforeDefault {
			get { return (_beforeDefault); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_beforeDefault == null);
				if (value.Parent != null)
					Errors.ThrowInternalError();
				value.Parent = this;
				_beforeDefault = value;
			}
		}

		public StatementListStatement DefaultClause {
			get { return (_defaultClause); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_defaultClause == null);
				if (value.Parent != null)
					value.Remove();
				value.Parent = this;
				_defaultClause = value;
			}
		}

		public CaseClauseBlockStatement AfterDefault {
			get { return (_afterDefault); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_afterDefault == null);
				if (value.Parent != null)
					Errors.ThrowInternalError();
				value.Parent = this;
				_afterDefault = value;
			}
		}
	}
}
