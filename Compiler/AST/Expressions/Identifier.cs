using System.Diagnostics.Contracts;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class Identifier : Expression {
		internal Identifier(string value)
			: base(ExpressionType.Ident) {
			Contract.Requires(!string.IsNullOrEmpty(value));
			Value = value;
		}

		public override string ToString() {
			return (Value);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (isLastOperator)
				return;
			compiler.Emitter.Emit(OpCode.LdLocal, Value);
		}

		public override bool IsReference {
			get { return (true); }
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool CanHaveMutableMembers {
			get { return (true); }
		}

		public override bool CanBeConstructor {
			get { return (true); }
		}

		public override bool CanBeFunction {
			get { return (true); }
		}

		public override bool CanBeDeleted {
			get { return (true); }
		}

		public override bool CanBeObject {
			get { return (true); }
		}

		public string Value { get; private set; }
	}
}
