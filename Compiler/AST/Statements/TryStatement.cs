using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор try (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.14)
	/// </summary>
	public sealed class TryStatement : LanguageStatement {
		private TryBlockStatement _tryBlock;
		private string _catchBlockVariable;
		private BlockStatement _catchBlock;
		private BlockStatement _finallyBlock;

		public TryStatement(int lineNo)
			: base(StatementType.Try, lineNo) {
		}

		internal override void Preprocess(FunctionCompiler compiler) {
			if (_catchBlock == null && _finallyBlock == null)
				Errors.ThrowInternalError();
			compiler.TryStatements.Add(this);
		}

		internal void EmitTryCatch(FunctionCompiler compiler) {
			Contract.Assert(_catchBlock != null);

			var catchLabel = compiler.Emitter.DefineLabel();
			compiler.Emitter.Emit(OpCode.EnterTry, catchLabel);
			_tryBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.LeaveTry);

			compiler.Emitter.MarkLabel(catchLabel);
			compiler.Emitter.Emit(OpCode.EnterCatch, _catchBlockVariable);
			_catchBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.LeaveCatch);
		}

		internal void EmitTryFinally(FunctionCompiler compiler) {
			Contract.Assert(_finallyBlock != null);

			var finallyLabel = compiler.Emitter.DefineLabel();
			compiler.Emitter.Emit(OpCode.EnterTry, finallyLabel);
			_tryBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.LeaveTry);

			compiler.Emitter.MarkLabel(finallyLabel);
			_finallyBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.Rethrow);
		}

		internal void EmitTryCatchFinally(FunctionCompiler compiler) {
			Contract.Assert(_catchBlock != null && _finallyBlock != null);

			var finallyLabel = compiler.Emitter.DefineLabel();
			compiler.Emitter.Emit(OpCode.EnterTry, finallyLabel);

			var catchLabel = compiler.Emitter.DefineLabel();
			compiler.Emitter.Emit(OpCode.EnterTry, catchLabel);
			_tryBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.LeaveTry);

			compiler.Emitter.MarkLabel(catchLabel);
			compiler.Emitter.Emit(OpCode.EnterCatch, _catchBlockVariable);
			_catchBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.LeaveCatch);

			compiler.Emitter.Emit(OpCode.LeaveTry);

			compiler.Emitter.MarkLabel(finallyLabel);
			_finallyBlock.CompileBy(compiler);
			compiler.Emitter.Emit(OpCode.Rethrow);
		}

		internal override void CompileBy(FunctionCompiler compiler) {
			if (_catchBlock == null && _finallyBlock == null)
				Errors.ThrowInternalError();

			if (_catchBlock != null) {
				if (_finallyBlock == null)
					EmitTryCatch(compiler);
				else
					EmitTryCatchFinally(compiler);
			}
			else {
				EmitTryFinally(compiler);
			}
		}

		public TryBlockStatement TryBlock {
			get { return (_tryBlock); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_tryBlock == null);
				if (value.Parent != null)
					value.Remove();
				value.Parent = this;
				_tryBlock = value;
			}
		}

		public string CatchBlockVariable {
			get { return (_catchBlockVariable); }
			set {
				Contract.Requires(!string.IsNullOrEmpty(value));
				Contract.Assert(_catchBlock == null);
				_catchBlockVariable = value;
			}
		}

		public BlockStatement CatchBlock {
			get { return (_catchBlock); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_catchBlock == null);
				if (value.Parent != null)
					value.Remove();
				value.Parent = this;
				_catchBlock = value;
			}
		}

		public BlockStatement FinallyBlock {
			get { return (_finallyBlock); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_finallyBlock == null);
				if (value.Parent != null)
					value.Remove();
				value.Parent = this;
				_finallyBlock = value;
			}
		}
	}
}
