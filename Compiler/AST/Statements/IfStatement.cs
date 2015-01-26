using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор if (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.5)
	/// </summary>
	public sealed class IfStatement : LanguageStatement {
		private Expression _condition;
		private Statement _thenStatement;
		private Statement _elseStatement;

		public IfStatement(int lineNo)
			: base(StatementType.If, lineNo) {
		}

		protected internal override void InsertBefore(Statement position, Statement newStatement) {
			Statement target;
			if (ReferenceEquals(position, _thenStatement)) {
				if (_thenStatement.Type != StatementType.Compound)
					_thenStatement = _thenStatement.WrapToBlock();
				target = _thenStatement;
			}
			else {
				Contract.Assert(ReferenceEquals(position, _elseStatement));
				if (_elseStatement.Type != StatementType.Compound)
					_elseStatement = _elseStatement.WrapToBlock();
				target = _elseStatement;
			}
			target.InsertBefore(position, newStatement);
		}

		protected override void Remove(Statement child) {
			if (ReferenceEquals(_thenStatement, child))
				_thenStatement = null;
			else {
				Contract.Assert(ReferenceEquals(_elseStatement, child));
				_elseStatement = null;
			}
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			if (_thenStatement == null)
				Errors.ThrowInternalError();
			else
				_thenStatement.Preprocess(compiler);
			if (_elseStatement != null)
				_elseStatement.Preprocess(compiler);
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			Condition.CompileBy(compiler, false);
			if (_elseStatement == null) {
				var endLabel = compiler.Emitter.DefineLabel();
				compiler.Emitter.Emit(OpCode.GotoIfFalse, endLabel);
				ThenStatement.CompileBy(compiler);
				compiler.Emitter.MarkLabel(endLabel);
			}
			else {
				var endLabel = compiler.Emitter.DefineLabel();
				var falseLabel = compiler.Emitter.DefineLabel();
				compiler.Emitter.Emit(OpCode.GotoIfFalse, falseLabel);
				_thenStatement.CompileBy(compiler);
				compiler.Emitter.Emit(OpCode.Goto, endLabel);
				compiler.Emitter.MarkLabel(falseLabel);
				_elseStatement.CompileBy(compiler);
				compiler.Emitter.MarkLabel(endLabel);
			}
		}

		public Expression Condition {
			get { return (_condition); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_condition == null);
				_condition = value;
			}
		}

		public Statement ThenStatement {
			get { return (_thenStatement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_thenStatement == null);
				if (value.Parent != null)
					value.Parent.Remove();
				value.Parent = this;
				_thenStatement = value;
			}
		}

		public Statement ElseStatement {
			get { return (_elseStatement); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_elseStatement == null);
				if (value.Parent != null)
					value.Parent.Remove();
				value.Parent = this;
				_elseStatement = value;
			}
		}
	}
}
