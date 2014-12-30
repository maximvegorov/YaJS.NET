using System.Diagnostics.Contracts;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class Identifier : Expression {
		public Identifier(string value) {
			Contract.Requires(!string.IsNullOrEmpty(value));
			Value = value;
		}

		public override string ToString() {
			return (Value);
		}

		public override bool IsReference { get { return (true); } }
		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeDeleted { get { return (true); } }

		public string Value { get; set; } 
	}
}
