using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace YaJS.Runtime.Exceptions {
	/// <summary>
	/// В данное исключение отображаются все отличные от RuntimeErrorException и его подклассов
	/// исключения возникающие в ходе выполнения потока. После возникновения данного исключения поток
	/// не может больше использоваться и должен быть завершен
	/// </summary>
	[Serializable]
	public sealed class UnrecoverableErrorException : RuntimeException {
		public UnrecoverableErrorException(string message, CallStackFrameView[] stackTraceView, Exception innerException)
			: base(message, innerException) {
			Contract.Requires(stackTraceView != null);
			StackTraceView = stackTraceView;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
			info.AddValue("StackTraceView", StackTraceView);
		}

		public CallStackFrameView[] StackTraceView { get; private set; }
	}
}
