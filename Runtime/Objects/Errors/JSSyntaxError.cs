namespace YaJS.Runtime.Objects.Errors {
	public sealed class JSSyntaxError : JSError {
		internal JSSyntaxError(string message, JSObject inherited)
			: base(message, inherited) {
		}
	}
}
