using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Identifier : Expression {
		private readonly string _value;

		public Identifier(string value)
			: base(ExpressionType.Ident) {
			Contract.Requires(!string.IsNullOrEmpty(value));
			_value = value;
		}

		public override string ToString() {
			return (_value);
		}

		public override bool IsReference { get { return (true); } }
		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeDeleted { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
