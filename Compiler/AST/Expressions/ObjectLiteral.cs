using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class ObjectLiteral : Expression {
		internal ObjectLiteral(List<KeyValuePair<string, Expression>> properties)
			: base(ExpressionType.ObjectLiteral) {
			Contract.Requires(properties != null);
			Properties = properties;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('{');
			if (Properties.Count > 0) {
				foreach (var property in Properties)
					result.Append('"').Append(property.Key).Append('"').Append(':').Append(property.Value).Append(',');
				result.Length -= 1;
			}
			result.Append('}');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			if (Properties.Count == 0) {
				if (isLastOperator)
					return;
				compiler.Emitter.Emit(OpCode.MakeEmptyObject);
			}
			else {
				foreach (var property in Properties) {
					property.Value.CompileBy(compiler, isLastOperator);
					compiler.Emitter.Emit(OpCode.LdString, property.Key);
				}
				compiler.Emitter.Emit(OpCode.LdInteger, Properties.Count);
				compiler.Emitter.Emit(OpCode.MakeObject);
				if (isLastOperator)
					compiler.Emitter.Emit(OpCode.Pop);
			}
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool CanHaveMutableMembers {
			get { return (true); }
		}

		public override bool CanBeObject {
			get { return (true); }
		}

		public List<KeyValuePair<string, Expression>> Properties { get; private set; }
	}
}
