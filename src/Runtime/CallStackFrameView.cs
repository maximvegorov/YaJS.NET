using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace YaJS.Runtime {
	[Serializable]
	public sealed class CallStackFrameView {
		public CallStackFrameView(CallStackFrame stackFrame) {
			Contract.Requires(stackFrame != null);
			FunctionName = stackFrame.Function.CompiledFunction.Name;
			CodeOffset = stackFrame.CodeReader.Offset;
		}

		public override string ToString() {
			return (string.Format("{0} at {1}", FunctionName, CodeOffset.ToString(CultureInfo.InvariantCulture)));
		}

		public string FunctionName { get; private set; }
		public int CodeOffset { get; private set; }
	}
}
