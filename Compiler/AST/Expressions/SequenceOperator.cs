using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class SequenceOperator : Expression {
		internal SequenceOperator(List<Expression> operands)
			: base(ExpressionType.Sequence) {
			Contract.Requires(operands != null && operands.Count > 1);
			Operands = operands;
		}

		public override string ToString() {
			var result = new StringBuilder();
			foreach (var operand in Operands)
				result.Append('(').Append(operand).Append('(').Append(',');
			result.Length -= 1;
			return (result.ToString());
		}

		public override bool Equals(object obj) {
			var other = obj as SequenceOperator;
			return (other != null && Operands.SequenceEqual(other.Operands));
		}

		public override int GetHashCode() {
			return (GetHashCode(Type.GetHashCode(), GetHashCode(Operands.Select(o => o.GetHashCode()))));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			for (var i = 0; i < Operands.Count - 1; i++)
				Operands[i].CompileBy(compiler, true);
			Operands[Operands.Count - 1].CompileBy(compiler, isLast);
		}

		public override bool CanHaveMembers {
			get { return (Operands[Operands.Count - 1].CanHaveMembers); }
		}

		public override bool CanHaveMutableMembers {
			get { return (Operands[Operands.Count - 1].CanHaveMutableMembers); }
		}

		public override bool CanBeConstructor {
			get { return (Operands[Operands.Count - 1].CanBeConstructor); }
		}

		public override bool CanBeFunction {
			get { return (Operands[Operands.Count - 1].CanBeFunction); }
		}

		public override bool CanBeDeleted {
			get { return (Operands[Operands.Count - 1].CanBeDeleted); }
		}

		public override bool CanBeObject {
			get { return (Operands[Operands.Count - 1].CanBeObject); }
		}

		public override bool IsConstant {
			get { return (Operands.All(o => o.IsConstant)); }
		}

		public List<Expression> Operands { get; private set; }
	}
}
