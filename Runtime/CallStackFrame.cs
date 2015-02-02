using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Objects;
using YaJS.Runtime.Values;

namespace YaJS.Runtime {
	/// <summary>
	/// Кадр вызова функции
	/// </summary>
	public sealed class CallStackFrame {
		/// <summary>
		/// Стек вычислений
		/// </summary>
		private readonly Stack<JSValue> _evalStack;
		/// <summary>
		/// Текущий блок try
		/// </summary>
		private TryBlockInfo _currentTryBlock;

		internal CallStackFrame(
			CallStackFrame caller,
			VirtualMachine vm,
			JSManagedFunction function,
			JSObject context,
			VariableScope localScope,
			JSValue[] parameterValues,
			bool copyResult = false,
			Action onCompleteCallback = null) {
			Contract.Requires(vm != null);
			Contract.Requires(function != null);
			Contract.Requires(context != null);
			Contract.Requires(localScope != null);
			Contract.Requires(parameterValues != null);

			Caller = caller;
			CopyResult = copyResult;
			Function = function;
			Context = context;
			LocalScope = localScope;

			_evalStack = new Stack<JSValue>(4);

			CodeReader = new ByteCodeReader(Function.CompiledFunction.CompiledCode);

			OnCompleteCallback = onCompleteCallback;

			// Создать привязки для параметров
			var parameterNames = function.CompiledFunction.ParameterNames;
			var n = Math.Min(parameterNames.Length, parameterValues.Length);
			for (var i = 0; i < n; i++)
				LocalScope.DeclareVariable(parameterNames[i], parameterValues[i]);
			for (var i = n; i < parameterNames.Length; i++)
				LocalScope.DeclareVariable(parameterNames[i], JSValue.Undefined);

			// Создать привязку для arguments
			LocalScope.DeclareVariable("arguments", vm.NewArguments(parameterValues));

			// Создать привязки для объявленных функций
			for (var i = 0; i < Function.CompiledFunction.DeclaredFunctionCount; i++) {
				var declaredFunction = Function.CompiledFunction.NestedFunctions[i];
				if (!LocalScope.ContainsVariable(declaredFunction.Name))
					LocalScope.DeclareVariable(declaredFunction.Name, vm.NewFunction(LocalScope, declaredFunction));
			}

			// Создать привязки для объявленных переменных
			for (var i = 0; i < Function.CompiledFunction.DeclaredVariables.Length; i++) {
				var variableName = Function.CompiledFunction.DeclaredVariables[i];
				LocalScope.DeclareVariableIfNotExists(variableName, JSValue.Undefined);
			}
		}

		internal void Push(JSValue value) {
			_evalStack.Push(value);
		}

		internal void Push(JSNumberValue value) {
			_evalStack.Push(value);
		}

		internal JSValue Peek() {
			return (_evalStack.Peek());
		}

		internal JSValue Pop() {
			return (_evalStack.Pop());
		}

		internal JSValue[] PopArguments() {
			var argumentCount = _evalStack.Pop().RequireInteger();
			Contract.Assert(argumentCount >= 0);
			var result = new JSValue[argumentCount];
			for (var i = argumentCount - 1; i >= 0; i--)
				result[i] = _evalStack.Pop();
			return (result);
		}

		internal JSFunction GetFunction(VirtualMachine vm, int index) {
			return (vm.NewFunction(LocalScope, Function.CompiledFunction.NestedFunctions[index]));
		}

		internal void BeginScope() {
			LocalScope = new LocalVariableScope(LocalScope);
		}

		internal void EndScope() {
			Contract.Ensures(LocalScope != null);
			LocalScope = LocalScope.OuterScope;
		}

		internal void EnterTry() {
			_currentTryBlock = new TryBlockInfo(CodeReader.ReadInteger(), LocalScope, _currentTryBlock);
		}

		internal void LeaveTry() {
			if (_currentTryBlock == null)
				throw new IllegalOpCodeException(OpCode.LeaveTry.ToString());
			_currentTryBlock = _currentTryBlock.OuterBlock;
			CodeReader.Seek(CodeReader.ReadInteger());
		}

		internal bool TryHandle(ExceptionObject exception) {
			if (_currentTryBlock == null)
				return (false);
			LocalScope = _currentTryBlock.Scope;
			CodeReader.Seek(_currentTryBlock.HandlerOffset);
			_currentTryBlock = _currentTryBlock.OuterBlock;
			_evalStack.Push(exception.ThrownValue);
			_evalStack.Push((JSNumberValue)1);
			return (true);
		}

		private IEnumerable<CallStackFrame> GetFrames() {
			for (var frame = this; frame != null; frame = frame.Caller)
				yield return frame;
		}

		public CallStackFrameView[] ToStackTrace() {
			return (GetFrames().Select(f => new CallStackFrameView(f)).Reverse().ToArray());
		}

		[Conditional("DEBUG")]
		internal void Dump() {
			Console.WriteLine("Code offset: {0:X8}", CodeReader.Offset);
			Console.WriteLine("EvalStack: {0}", _evalStack.Count);
			if (_evalStack.Count > 0) {
				var evalStackValues = new JSValue[_evalStack.Count];
				_evalStack.CopyTo(evalStackValues, 0);
				for (var i = evalStackValues.Length - 1; i >= 0; i--) {
					Console.WriteLine("[{0:D3}] - {1}", i, evalStackValues[i]);
				}
			}
			Console.WriteLine("Local variables: {0}", Function.CompiledFunction.DeclaredVariables.Length);
			for (var i = 0; i < Function.CompiledFunction.DeclaredVariables.Length; i++) {
				var variableName = Function.CompiledFunction.DeclaredVariables[i];
				Console.WriteLine("{0} = {1}", variableName, LocalScope.GetVariable(variableName));
			}
			Console.WriteLine();
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
		public VariableScope LocalScope { get; private set; }

		/// <summary>
		/// Byte-код reader
		/// </summary>
		internal ByteCodeReader CodeReader { get; private set; }

		/// <summary>
		/// Callback вызывающийся после завершения
		/// </summary>
		internal Action OnCompleteCallback { get; private set; }
	}
}
