using System.Globalization;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	internal sealed class StringLiteral : Expression {
		public StringLiteral(string value) {
			Value = value ?? string.Empty;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('"');
			for (var i = 0; i < result.Length; i++) {
				var c = Value[i];
				switch (c) {
					case '"':
						result.Append("\\\"");
						break;
					case '\\':
						result.Append("\\\\");
						break;
					case '\b':
						result.Append("\\b");
						break;
					case '\f':
						result.Append("\\f");
						break;
					case '\n':
						result.Append("\\n");
						break;
					case '\r':
						result.Append("\\r");
						break;
					case '\t':
						result.Append("\\t");
						break;
					case '\v':
						result.Append("\\v");
						break;
					default:
						if (!char.IsControl(c))
							result.Append(c);
						else {
							result.Append("\\u")
								.Append(((int)c).ToString("X4", CultureInfo.InvariantCulture));
						}
						break;
				}
			}
			result.Append('"');
			return (result.ToString());
		}

		public override bool CanHaveMembers { get { return (true); } }

		public string Value { get; private set; }
	}
}
