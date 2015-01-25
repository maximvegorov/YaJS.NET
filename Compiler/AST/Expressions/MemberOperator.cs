using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class MemberOperator : Expression {
		internal MemberOperator(Expression baseValue, Expression property)
			: base(ExpressionType.Member) {
			Contract.Requires(baseValue != null && baseValue.CanHaveMembers);
			Contract.Requires(property != null);
			BaseValue = baseValue;
			Property = property;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append(BaseValue);
			if (Property.Type == ExpressionType.Ident)
				result.Append('.').Append(Property);
			else
				result.Append('[').Append(Property).Append(']');
			return (result.ToString());
		}

		public override bool Equals(object obj) {
			var other = obj as MemberOperator;
			return (other != null && BaseValue.Equals(other.BaseValue) && Property.Equals(other.Property));
		}

		public override int GetHashCode() {
			return (GetHashCode(GetHashCode(Type.GetHashCode(), BaseValue.GetHashCode()), Property.GetHashCode()));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			BaseValue.CompileBy(compiler, false);
			Property.CompileBy(compiler, false);
			compiler.Emitter.Emit(OpCode.LdMember);
		}

		public override bool IsReference {
			get { return (BaseValue.CanHaveMutableMembers); }
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

		public Expression BaseValue { get; private set; }
		public Expression Property { get; private set; }
	}
}
