using System.Diagnostics.Contracts;

namespace YaJS.Compiler {
	public partial class Parser {
		public Parser(Tokenizer tokenizer) {
			Contract.Requires(tokenizer != null);
			Tokenizer = tokenizer;
			Tokenizer.ReadToken();
		}

		public Tokenizer Tokenizer { get; private set; }
	}
}
