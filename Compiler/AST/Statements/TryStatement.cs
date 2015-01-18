using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Statements {
	/// <summary>
	/// Оператор try (См. http://www.ecma-international.org/ecma-262/5.1/#sec-12.14)
	/// </summary>
	internal sealed class TryStatement : LanguageStatement {
		private readonly List<Statement> _exitPoints;
		private BlockStatement _catchBlock;
		private string _catchBlockVariable;
		private BlockStatement _finallyBlock;
		private BlockStatement _tryBlock;

		public TryStatement(Statement parent, int lineNo)
			: base(parent, StatementType.Try, lineNo) {
			_exitPoints = new List<Statement>();
		}

		protected override void RegisterAsExitPoint(Statement exitPoint) {
			_exitPoints.Add(exitPoint);
		}

		public BlockStatement TryBlock {
			get { return (_tryBlock); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_tryBlock == null);
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
				_catchBlock = value;
			}
		}

		public BlockStatement FinallyBlock {
			get { return (_finallyBlock); }
			set {
				Contract.Requires(value != null);
				Contract.Assert(_finallyBlock == null);
				_finallyBlock = value;
			}
		}
	}
}
