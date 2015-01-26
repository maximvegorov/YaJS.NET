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

		internal void CompilePropertyBy(FunctionCompiler compiler) {
			if (Property.Type == ExpressionType.Ident)
				compiler.Emitter.Emit(OpCode.LdString, ((Identifier)Property).Value);
			else
				Property.CompileBy(compiler, false);
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			BaseValue.CompileBy(compiler, false);
			CompilePropertyBy(compiler);
			compiler.Emitter.Emit(OpCode.LdMember);
			if (isLastOperator)
				compiler.Emitter.Emit(OpCode.Pop);
		}

		public override bool IsReference { get { return (BaseValue.CanHaveMutableMembers); } }
		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeConstructor { get { return (true); } }
		public override bool CanBeFunction { get { return (true); } }
		public override bool CanBeDeleted { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
		public Expression BaseValue { get; private set; }
		public Expression Property { get; private set; }
	}
}
