using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	public class GroupingOperator : Expression {
		internal GroupingOperator(Expression operand)
			: base(ExpressionType.Grouping) {
			Contract.Requires(operand != null);
			Operand = operand;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('(').Append(Operand).Append(')');
			return (result.ToString());
		}

		public override bool Equals(object obj) {
			var other = obj as GroupingOperator;
			return (other != null && Operand.Equals(other.Operand));
		}

		public override int GetHashCode() {
			return (GetHashCode(Type.GetHashCode(), Operand.GetHashCode()));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			Operand.CompileBy(compiler, isLast);
		}

		public override bool CanHaveMembers {
			get { return (Operand.CanHaveMembers); }
		}

		public override bool CanHaveMutableMembers {
			get { return (Operand.CanHaveMutableMembers); }
		}

		public override bool CanBeConstructor {
			get { return (Operand.CanBeConstructor); }
		}

		public override bool CanBeFunction {
			get { return (Operand.CanBeFunction); }
		}

		public override bool CanBeDeleted {
			get { return (Operand.CanBeDeleted); }
		}

		public override bool CanBeObject {
			get { return (Operand.CanBeObject); }
		}

		public override bool IsConstant {
			get { return (Operand.IsConstant); }
		}

		public Expression Operand { get; private set; }
	}
}
