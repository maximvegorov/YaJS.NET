using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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

		public override bool Equals(object obj) {
			var other = obj as ObjectLiteral;
			return (other != null && Properties.SequenceEqual(other.Properties));
		}

		public override int GetHashCode() {
			return (GetHashCode(Type.GetHashCode(), GetHashCode(Properties.Select(p => p.GetHashCode()))));
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			// Надо учесть возможность побочных эффектов вызова выражений
			foreach (var property in Properties) {
				property.Value.CompileBy(compiler, isLast);
				if (!isLast)
					compiler.Emitter.Emit(OpCode.LdString, property.Key);
			}
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdInteger, Properties.Count);
			compiler.Emitter.Emit(OpCode.MakeObject);
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
