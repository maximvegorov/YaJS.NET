using System.Diagnostics.Contracts;
using System.Globalization;

namespace YaJS.Compiler {
	internal static class Messages {
		internal static string Error(int lineNo, int columnNo, string message) {
			Contract.Requires(lineNo > 0);
			Contract.Requires(columnNo > 0);
			Contract.Requires(!string.IsNullOrEmpty(message));
			return (string.Format(
				"[Error] - {0} at {1},{2}",
				message,
				lineNo.ToString(CultureInfo.InvariantCulture),
				columnNo.ToString(CultureInfo.InvariantCulture)
			));
		}
	}
}
