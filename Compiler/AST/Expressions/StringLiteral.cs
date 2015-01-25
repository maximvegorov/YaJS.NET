using System.Globalization;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class StringLiteral : Expression {
		internal StringLiteral(string value)
			: base(ExpressionType.StringLiteral) {
			Value = value ?? string.Empty;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('"');
			for (var i = 0; i < Value.Length; i++) {
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
						else
							result.Append("\\u").Append(((int)c).ToString("X4", CultureInfo.InvariantCulture));
						break;
				}
			}
			result.Append('"');
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLast) {
			if (isLast)
				return;
			compiler.Emitter.Emit(OpCode.LdString, Value);
		}

		public override bool CanHaveMembers {
			get { return (true); }
		}

		public override bool IsConstant {
			get { return (true); }
		}

		public override bool CanBeUsedInCaseClause {
			get { return (true); }
		}

		public string Value { get; private set; }
	}
}
