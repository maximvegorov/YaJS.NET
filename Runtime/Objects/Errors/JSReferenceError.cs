namespace YaJS.Runtime.Objects.Errors {
	public sealed class JSReferenceError : JSError {
		internal JSReferenceError(string message, JSObject inherited)
			: base(message, inherited) {
		}
	}
}
