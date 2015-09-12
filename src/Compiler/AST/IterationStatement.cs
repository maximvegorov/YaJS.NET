using System.Diagnostics.Contracts;
using YaJS.Compiler.Emitter;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Цикл (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.6)
	/// </summary>
	public abstract class IterationStatement : LabellableStatement {
		internal class CompilingContext {
			public CompilingContext(FunctionCompiler compiler) {
				Compiler = compiler;
				StartLabel = compiler.Emitter.DefineLabel();
				EndLabel = compiler.Emitter.DefineLabel();
			}

			public FunctionCompiler Compiler { get; private set; }
			public Label StartLabel { get; private set; }
			public Label EndLabel { get; private set; }
		}

		private Statement _statement;

		protected IterationStatement(StatementType type, int lineNo, ILabelSet labelSet)
			: base(type, lineNo, labelSet) {
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			if (_statement.Type != StatementType.Compound)
				_statement = _statement.WrapToBlock();
			_statement.InsertBefore(position, newStatement);
		}

		protected override void Remove(Statement child) {
			if (ReferenceEquals(_statement, child))
				_statement = null;
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			if (_statement == null)
				Errors.ThrowInternalError();
			else
				_statement.Preprocess(compiler);
		}

		internal virtual void DoEmitProlog(FunctionCompiler compiler) {
		}

		internal abstract void DoEmit(CompilingContext context);

		internal virtual void DoEmitEpilog(FunctionCompiler compiler) {
		}

		internal override sealed void CompileBy(FunctionCompiler compiler) {
			DoEmitProlog(compiler);
			var context = new CompilingContext(compiler);
			try {
				compiler.StatementStarts.Add(this, context.StartLabel);
				compiler.StatementEnds.Add(this, context.EndLabel);
				compiler.Emitter.MarkLabel(context.StartLabel);
				DoEmit(context);
				compiler.Emitter.MarkLabel(context.EndLabel);
			}
			finally {
				compiler.StatementEnds.Remove(this);
				compiler.StatementStarts.Remove(this);
			}
			DoEmitEpilog(compiler);
			compiler.MarkEndOfStatement();
		}

		public Statement Statement {
			get { return (_statement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_statement == null);
				if (value.Parent != null)
					value.Parent.Remove();
				value.Parent = this;
				_statement = value;
			}
		}
	}
}
