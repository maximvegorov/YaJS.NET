using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class ObjectLiteral : Expression {
		private readonly List<KeyValuePair<string, Expression>> _properties;

		public ObjectLiteral(List<KeyValuePair<string, Expression>> properties) : base(ExpressionType.ObjectLiteral) {
			Contract.Requires(properties != null);
			_properties = properties;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('{');
			if (_properties.Count > 0) {
				foreach (var property in _properties) {
					result.Append('"').Append(property.Key).Append('"')
						.Append(':')
						.Append(property.Value)
						.Append(',');
				}
				result.Length -= 1;
			}
			result.Append('}');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }
		public override bool CanHaveMutableMembers { get { return (true); } }
		public override bool CanBeObject { get { return (true); } }
	}
}
