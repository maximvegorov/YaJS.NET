using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class AndOperator : BinaryOperator {
		public AndOperator(Expression leftOperand, Expression rightOperand)
			: base(ExpressionType.And, leftOperand, rightOperand) {
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(LeftOperand).Append(" && ").Append(RightOperand);
			return (result.ToString());
		}

		public override bool CanHaveMembers {
			get { return (LeftOperand.CanHaveMembers || RightOperand.CanHaveMembers); }
		}

		public override bool CanHaveMutableMembers {
			get { return (LeftOperand.CanHaveMutableMembers || RightOperand.CanHaveMutableMembers); }
		}

		public override bool CanBeConstructor {
			get { return (LeftOperand.CanBeConstructor || RightOperand.CanBeConstructor); }
		}

		public override bool CanBeFunction {
			get { return (LeftOperand.CanBeFunction || RightOperand.CanBeFunction); }
		}

		public override bool CanBeObject {
			get { return (LeftOperand.CanBeObject || RightOperand.CanBeObject); }
		}
	}
}
