using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Runtime {
	using Runtime.Exceptions;
	using Runtime.Objects;

	/// <summary>
	/// Кадр вызова функции
	/// </summary>
	public sealed class CallStackFrame {
		internal CallStackFrame(
			VirtualMachine vm,
			JSManagedFunction function,
			JSObject context,
			List<JSValue> argumentValues,
			CallStackFrame caller = null,
			bool copyResult = false
		) {
			Contract.Requires(vm != null);
			Contract.Requires(function != null);
			Contract.Requires(context != null);
			Contract.Requires(argumentValues != null);
			Contract.Ensures(
				LocalScope != null &&
				LocalScope.Variables.Count == Math.Max(function.CompiledFunction.Arguments.Length, LocalScope.Variables.Count) + 1
			);

			Caller = caller;
			CopyResult = copyResult;
			Function = function;
			Context = context;

			LocalScope = new LocalScope(function.OuterScope);

			EvalStack = new Stack<JSValue>(4);

			// Поместить параметры в область хранения локальных переменных
			var argumentNames = function.CompiledFunction.Arguments;
			LocalScope.Variables.Add("arguments", vm.NewArray(argumentValues));
			var n = Math.Min(argumentNames.Length, argumentValues.Count);
			for (var i = 0; i < n; i++) {
				LocalScope.Variables.Add(argumentNames[i], argumentValues[i]);
			}
			for (var i = n; i < argumentNames.Length; i++) {
				LocalScope.Variables.Add(argumentNames[i], JSValue.Undefined);
			}

			CodeReader = new ByteCodeReader(Function.CompiledFunction.CompiledCode);
		}

		internal void Push(JSValue value) {
			EvalStack.Push(value);
		}

		internal JSValue Peek() {
			return (EvalStack.Peek());
		}

		internal JSValue Pop() {
			return (EvalStack.Pop());
		}

		internal JSValue PopPrimitiveValue() {
			return (JSValue.ToPrimitiveValue(EvalStack.Pop()));
		}

		internal List<JSValue> PopArguments() {
			var result = new List<JSValue>(EvalStack.Pop().GetAsInteger());
			for (var i = result.Count - 1; i >= 0; i--)
				result[i] = EvalStack.Pop();
			return (result);
		}

		internal JSFunction GetLocalFunction(VirtualMachine vm, int index) {
			return (vm.NewFunction(
				Function.CompiledFunction.NestedFunctions[index], LocalScope
			));
		}

		internal void BeginScope() {
			LocalScope = new LocalScope(LocalScope);
		}

		internal void EndScope() {
			Contract.Ensures(LocalScope != null);
			LocalScope = LocalScope.OuterScope;
		}

		internal void BeginTry() {
			CurrentTryBlock = new TryBlockInfo(CodeReader.ReadInteger(), LocalScope, CurrentTryBlock);
		}

		internal void EndTry() {
			if (CurrentTryBlock == null)
				throw new UnexpectedEndTryException();
			CurrentTryBlock = CurrentTryBlock.OuterBlock;
			CodeReader.Seek(CodeReader.ReadInteger());
		}

		internal bool TryHandle(ExceptionObject exception) {
			if (CurrentTryBlock == null)
				return (false);
			LocalScope = CurrentTryBlock.Scope;
			CodeReader.Seek(CurrentTryBlock.HandlerOffset);
			CurrentTryBlock = CurrentTryBlock.OuterBlock;
			EvalStack.Push(exception.ThrownValue);
			EvalStack.Push(JSValue.Create(1));
			return (true);
		}

		private IEnumerable<CallStackFrame> GetFrames() {
			for (var frame = this; frame != null; frame = frame.Caller) {
				yield return frame;
			}
		}

		public CallStackFrameView[] ToStackTrace() {
			return (
				GetFrames().Select(f => new CallStackFrameView(f))
					.Reverse()
					.ToArray()
			);
		}

		/// <summary>
		/// Стек-фрейм вызывающей функции
		/// </summary>
		public CallStackFrame Caller { get; private set; }
		/// <summary>
		/// Копировать возвращаемое значение в стек вычислений вызывающей функции
		/// </summary>
		public bool CopyResult { get; private set; }
		/// <summary>
		/// Вызванная функция
		/// </summary>
		public JSManagedFunction Function { get; private set; }
		/// <summary>
		/// Контекст в котором вызывается функция
		/// </summary>
		public JSObject Context { get; private set; }
		/// <summary>
		/// Область хранения локальных переменных
		/// </summary>
		public LocalScope LocalScope { get; private set; }
		/// <summary>
		/// Стек вычислений
		/// </summary>
		internal Stack<JSValue> EvalStack { get; private set; }
		/// <summary>
		/// Текущий блок try
		/// </summary>
		internal TryBlockInfo CurrentTryBlock { get; private set; }
		/// <summary>
		/// Byte-код reader
		/// </summary>
		internal ByteCodeReader CodeReader { get; private set; }
	}
}
