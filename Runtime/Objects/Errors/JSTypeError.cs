namespace YaJS.Runtime.Objects.Errors {
	public sealed class JSTypeError : JSError {
		internal JSTypeError(string message, JSObject inherited)
			: base(message, inherited) {
		}
	}
}
