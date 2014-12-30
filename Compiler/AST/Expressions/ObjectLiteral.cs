using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ObjectLiteral : Expression {
		public ObjectLiteral(List<KeyValuePair<string, Expression>> properties) {
			Contract.Requires(properties != null);
			Properties = properties;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('{');
			if (Properties.Count > 0) {
				foreach (var property in Properties) {
					result.Append('"').Append(property.Key).Append('"')
						.Append(':')
						.Append(property.Value.ToString())
						.Append(',');
				}
				result.Length -= 1;
			}
			result.Append('}');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }

		public List<KeyValuePair<string, Expression>> Properties { get; private set; }
	}
}
