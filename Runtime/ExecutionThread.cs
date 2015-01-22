using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime {
	/// <summary>
	/// Поток исполнения в рамках которого исполняется JS-функция
	/// </summary>
	public sealed class ExecutionThread {
		internal ExecutionThread(VirtualMachine vm, CompiledFunction globalFunction) {
			Contract.Requires(vm != null);
			Contract.Requires(globalFunction != null);
			VM = vm;
			GlobalScope = new LocalScope(vm.Global.OwnMembers);
			GlobalFunction = new JSManagedFunction(VM, GlobalScope, globalFunction, VM.Function);
			CurrentFrame = new CallStackFrame(null, VM, GlobalFunction, vm.Global, new List<JSValue>());
		}

		private void Switch(int tableIndex, JSValue selector) {
			Contract.Requires(selector.Type == JSValueType.Integer || selector.Type == JSValueType.String);
			var table = CurrentFrame.Function.CompiledFunction.SwitchJumpTables[tableIndex];
			var offset = table.Jump(selector);
			CurrentFrame.CodeReader.Seek(offset);
		}

		private void Unwind() {
			Contract.Requires(CurrentException != null);
			for (var frame = CurrentFrame; frame != null; frame = frame.Caller) {
				if (frame.TryHandle(CurrentException)) {
					CurrentFrame = frame;
					return;
				}
			}
			if (OnUnhandledException != null)
				OnUnhandledException.Invoke(CurrentException);
			throw new UnhandledExceptionException(CurrentException.ThrownValue.CastToString());
		}

		private void EnterCatch() {
			if (CurrentException == null)
				throw new IllegalOpCodeException(OpCode.EnterCatch.ToString());
			CurrentFrame.BeginScope();
			CurrentFrame.LocalScope.Variables.Add(
				CurrentFrame.CodeReader.ReadString(),
				CurrentException.ThrownValue
				);
		}

		private void LeaveCatch() {
			if (CurrentException == null)
				throw new IllegalOpCodeException(OpCode.LeaveCatch.ToString());
			CurrentFrame.EndScope();
			CurrentException = null;
		}

		private void Throw(JSValue thrownValue) {
			if (CurrentException == null || !ReferenceEquals(CurrentException.ThrownValue, thrownValue))
				CurrentException = new ExceptionObject(thrownValue, CurrentException);
			Unwind();
		}

		private void Rethrow() {
			if (CurrentException == null)
				throw new IllegalOpCodeException(OpCode.Rethrow.ToString());
			Unwind();
		}

		private void NewObject(JSFunction constructor, List<JSValue> args) {
			if (constructor.IsNative)
				CurrentFrame.Push(constructor.Construct(this, CurrentFrame.LocalScope, args));
			else {
				var newObject = new JSObject(VM, constructor.GetPrototype());
				CurrentFrame = new CallStackFrame(CurrentFrame, VM, constructor as JSManagedFunction, newObject, args, true);
				CurrentFrame.Push(newObject);
			}
		}

		private void MakeObject(int memberCount) {
			var newObject = VM.NewObject();
			var members = newObject.OwnMembers;
			for (var i = memberCount - 1; i >= 0; i--) {
				var memberName = CurrentFrame.Pop().CastToString();
				var memberValue = CurrentFrame.Pop();
				members.Add(memberName, memberValue);
			}
			CurrentFrame.Push(newObject);
		}

		private void MakeArray(int memberCount) {
			var items = new List<JSValue>(memberCount);
			for (var i = memberCount - 1; i >= 0; i--)
				items[i] = CurrentFrame.Pop();
			CurrentFrame.Push(VM.NewArray(items));
		}

		/// <summary>
		/// Инициировать вызов функции (управляемой/неуправляемой)
		/// </summary>
		/// <param name="function">Функция</param>
		/// <param name="context">Контекст</param>
		/// <param name="args">Параметры</param>
		/// <param name="copyResult">Копировать результат работы функции в стек вычислений вызывающей функции</param>
		/// <param name="onCompleteCallback">Вызывается по завершении работы функции</param>
		internal void CallFunction(
			JSFunction function,
			JSObject context,
			List<JSValue> args,
			bool copyResult,
			Action onCompleteCallback = null
			) {
			if (function.IsNative)
				CurrentFrame.Push(function.Invoke(this, context, CurrentFrame.LocalScope, args));
			else {
				CurrentFrame = new CallStackFrame(
					CurrentFrame,
					VM,
					function as JSManagedFunction,
					context,
					args,
					copyResult,
					onCompleteCallback
					);
			}
		}

		private void ExecuteStep(CallStackFrame currentFrame) {
			try {
				switch (currentFrame.CodeReader.ReadOpCode()) {
					case OpCode.Nop:
						break;

						#region Загрузка значений

					case OpCode.LdUndefined:
						currentFrame.Push(JSValue.Undefined);
						break;
					case OpCode.LdNull:
						currentFrame.Push(JSValue.Null);
						break;
					case OpCode.LdBoolean:
						currentFrame.Push(currentFrame.CodeReader.ReadBoolean());
						break;
					case OpCode.LdInteger:
						currentFrame.Push(currentFrame.CodeReader.ReadInteger());
						break;
					case OpCode.LdFloat:
						currentFrame.Push(currentFrame.CodeReader.ReadFloat());
						break;
					case OpCode.LdString:
						currentFrame.Push(currentFrame.CodeReader.ReadString());
						break;

					case OpCode.LdThis:
						currentFrame.Push(CurrentFrame.Context);
						break;

						#endregion

						#region Локальные переменные

					case OpCode.LdLocal:
						currentFrame.Push(
							currentFrame.LocalScope.GetVariable(currentFrame.CodeReader.ReadString())
							);
						break;
					case OpCode.LdLocalFunc:
						currentFrame.Push(
							currentFrame.GetFunction(VM, currentFrame.CodeReader.ReadInteger())
							);
						break;
					case OpCode.StLocal:
						currentFrame.LocalScope.SetVariable(
							currentFrame.CodeReader.ReadString(),
							currentFrame.Pop()
							);
						break;
					case OpCode.DelLocal:
						currentFrame.Push(
							currentFrame.LocalScope.DeleteVariable(currentFrame.CodeReader.ReadString())
							);
						break;

						#endregion

						#region Свойства объектов

					case OpCode.IsMember: {
						var member = currentFrame.Pop();
						var obj = currentFrame.Pop().ToObject(VM);
						currentFrame.Push(obj.ContainsMember(member));
						break;
					}
					case OpCode.LdMember: {
						var member = currentFrame.Pop();
						var obj = currentFrame.Pop().ToObject(VM);
						currentFrame.Push(obj.GetMember(member));
						break;
					}
					case OpCode.StMember: {
						var member = currentFrame.Pop();
						var obj = currentFrame.Pop().RequireObject();
						obj.SetMember(member, currentFrame.Pop());
						break;
					}
					case OpCode.DelMember: {
						var member = currentFrame.Pop();
						var obj = currentFrame.Pop().RequireObject();
						currentFrame.Push(obj.DeleteMember(member));
						break;
					}

						#endregion

						#region Управляющие

					case OpCode.Dup:
						currentFrame.Push(currentFrame.Peek());
						break;
					case OpCode.Pop:
						currentFrame.Pop();
						break;

					case OpCode.Goto:
						currentFrame.CodeReader.Seek(currentFrame.CodeReader.ReadInteger());
						break;
					case OpCode.GotoIfTrue: {
						var offset = currentFrame.CodeReader.ReadInteger();
						if (currentFrame.Pop().CastToBoolean())
							currentFrame.CodeReader.Seek(offset);
						break;
					}
					case OpCode.GotoIfFalse: {
						var offset = currentFrame.CodeReader.ReadInteger();
						if (!currentFrame.Pop().CastToBoolean())
							currentFrame.CodeReader.Seek(offset);
						break;
					}

					case OpCode.Switch:
						Switch(
							currentFrame.CodeReader.ReadInteger(),
							currentFrame.Pop()
							);
						break;

					case OpCode.BeginScope:
						currentFrame.BeginScope();
						break;
					case OpCode.EndScope:
						currentFrame.EndScope();
						break;

					case OpCode.EnterTry:
						currentFrame.EnterTry();
						break;
					case OpCode.LeaveTry:
						currentFrame.LeaveTry();
						break;

					case OpCode.EnterCatch:
						EnterCatch();
						break;
					case OpCode.LeaveCatch:
						LeaveCatch();
						break;

					case OpCode.Throw:
						Throw(currentFrame.Pop());
						break;
					case OpCode.Rethrow:
						Rethrow();
						break;

						#endregion

						#region Вызовы функций

					case OpCode.NewObj: {
						var arguments = currentFrame.PopArguments();
						var constructor = currentFrame.Pop().RequireFunction();
						NewObject(constructor, arguments);
						break;
					}

					case OpCode.MakeEmptyObject:
						currentFrame.Push(VM.NewObject());
						break;
					case OpCode.MakeObject:
						MakeObject(currentFrame.Pop().RequireInteger());
						break;
					case OpCode.MakeEmptyArray:
						currentFrame.Push(VM.NewArray(new List<JSValue>()));
						break;
					case OpCode.MakeArray:
						MakeArray(currentFrame.Pop().RequireInteger());
						break;

					case OpCode.Call: {
						var arguments = currentFrame.PopArguments();
						var function = currentFrame.Pop().RequireFunction();
						CallFunction(
							function,
							VM.Global,
							arguments,
							currentFrame.CodeReader.ReadBoolean()
							);
						break;
					}

					case OpCode.CallMember: {
						var arguments = currentFrame.PopArguments();
						var context = currentFrame.Pop().ToObject(VM);
						var function = currentFrame.Pop().RequireFunction();
						CallFunction(
							function,
							context,
							arguments,
							currentFrame.CodeReader.ReadBoolean()
							);
						break;
					}

					case OpCode.Return:
						if (currentFrame.CopyResult) {
							Contract.Assert(currentFrame.Caller != null);
							CurrentFrame.Caller.Push(currentFrame.Pop());
						}
						CurrentFrame = currentFrame.Caller;
						if (currentFrame.OnCompleteCallback != null)
							currentFrame.OnCompleteCallback();
						break;

						#endregion

						#region Перечислители

					case OpCode.GetEnumerator:
						currentFrame.Push(currentFrame.Pop().GetJSEnumerator());
						break;
					case OpCode.EnumMoveNext: {
						var enumerator = currentFrame.Peek().RequireEnumerator();
						var hasMoreValue = enumerator.MoveNext();
						currentFrame.LocalScope.SetVariable(
							currentFrame.CodeReader.ReadString(),
							hasMoreValue ? enumerator.Current : JSValue.Undefined
							);
						currentFrame.Push(hasMoreValue);
						break;
					}

						#endregion

						#region Преобразования

					case OpCode.CastToPrimitive: {
						var obj = currentFrame.Pop();
						if (obj.Type == JSValueType.Object)
							obj.CastToPrimitiveValue(this, currentFrame.Push);
						break;
					}

						#endregion

						#region Числовые

					case OpCode.Inc:
						currentFrame.Push(currentFrame.Pop().ToNumber().Inc());
						break;
					case OpCode.Dec:
						currentFrame.Push(currentFrame.Pop().ToNumber().Dec());
						break;

					case OpCode.Pos:
						currentFrame.Push(currentFrame.Pop().ToNumber());
						break;
					case OpCode.Neg:
						currentFrame.Push(currentFrame.Pop().ToNumber().Neg());
						break;

					case OpCode.Plus: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.Plus(op2));
						break;
					}
					case OpCode.Minus: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.Minus(op2));
						break;
					}
					case OpCode.Mul: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.Mul(op2));
						break;
					}
					case OpCode.IntDiv: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.IntDiv(op2));
						break;
					}
					case OpCode.FltDiv: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.FltDiv(op2));
						break;
					}
					case OpCode.Mod: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.Mod(op2));
						break;
					}

					case OpCode.BitNot:
						currentFrame.Push(currentFrame.Pop().ToNumber().BitNot());
						break;
					case OpCode.BitAnd: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.BitAnd(op2));
						break;
					}
					case OpCode.BitOr: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.BitOr(op2));
						break;
					}
					case OpCode.BitXor: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.BitXor(op2));
						break;
					}

					case OpCode.Shl: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.BitShl(op2));
						break;
					}
					case OpCode.ShrS: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.BitShrS(op2));
						break;
					}
					case OpCode.ShrU: {
						var op1 = currentFrame.Pop().ToNumber();
						var op2 = currentFrame.Pop().ToNumber();
						currentFrame.Push(op1.BitShrU(op2));
						break;
					}

						#endregion

						#region Логические

					case OpCode.Not:
						currentFrame.Push(currentFrame.Pop().Not());
						break;
					case OpCode.And: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.And(op2));
						break;
					}
					case OpCode.Or: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.Or(op2));
						break;
					}

						#endregion

						#region Равенства

					case OpCode.ConvEq: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.ConvEqualsTo(op2));
						break;
					}
					case OpCode.StrictEq: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.StrictEqualsTo(op2));
						break;
					}
					case OpCode.ConvNeq: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(!op1.ConvEqualsTo(op2));
						break;
					}
					case OpCode.StrictNeq: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(!op1.StrictEqualsTo(op2));
						break;
					}

						#endregion

						#region Отношения

					case OpCode.Lt: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.Lt(op2));
						break;
					}
					case OpCode.Lte: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op1.Lte(op2));
						break;
					}
					case OpCode.Gt: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op2.Lt(op1));
						break;
					}
					case OpCode.Gte: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop();
						currentFrame.Push(op2.Lte(op1));
						break;
					}

					case OpCode.InstanceOf: {
						var op1 = currentFrame.Pop();
						var op2 = currentFrame.Pop().RequireFunction();
						currentFrame.Push(op1.IsInstanceOf(op2));
						break;
					}
					case OpCode.TypeOf:
						currentFrame.Push(currentFrame.Pop().TypeOf());
						break;

						#endregion

					default:
						throw new UnknownOpCodeException();
				}

				IsTerminated = CurrentFrame == null;
				if (IsTerminated)
					Result = currentFrame.Pop();
			}
			catch (RuntimeErrorException ex) {
				Throw(VM.NewInternalError(ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Вызвать функцию (управляемую/неуправляемую)
		/// </summary>
		/// <param name="function">Функция</param>
		/// <param name="context">Контекст</param>
		/// <param name="args">Параметры</param>
		public JSValue Invoke(
			JSFunction function,
			JSObject context,
			List<JSValue> args
			) {
			Contract.Requires<ArgumentNullException>(function != null, "function");
			Contract.Requires<ArgumentNullException>(context != null, "context");
			Contract.Requires<ArgumentNullException>(args != null, "args");
			var isCompleted = false;
			CallFunction(
				function,
				context,
				args,
				true,
				() => isCompleted = true
				);
			while (!isCompleted)
				ExecuteStep(CurrentFrame);
			return (CurrentFrame.Pop());
		}

		/// <summary>
		/// Выполнить следующую инструкцию
		/// </summary>
		/// <returns>False - если выполненная инструкция является последней, иначе true</returns>
		public bool ExecuteStep() {
			if (IsTerminated)
				throw new IllegalThreadStateException();

			var currentFrame = CurrentFrame;

			try {
				ExecuteStep(currentFrame);
				ExecutedStepCount++;
			}
			catch (Exception ex) {
				// Произошла серьезная ошибка связанная с внутренним состоянием потока.
				// Поток должен быть завершен. Дальнейшее его использование невозможно
				IsTerminated = true;
				throw new UnrecoverableErrorException(
					ex.Message,
					currentFrame.ToStackTrace(),
					ex
					);
			}

			return (IsTerminated);
		}

		/// <summary>
		/// Виртуальная машина
		/// </summary>
		public VirtualMachine VM { get; private set; }

		/// <summary>
		/// Область глобальных переменных
		/// </summary>
		public LocalScope GlobalScope { get; private set; }

		/// <summary>
		/// Исполняемая функция
		/// </summary>
		public JSManagedFunction GlobalFunction { get; private set; }

		/// <summary>
		/// Текуший кадр вызова
		/// </summary>
		public CallStackFrame CurrentFrame { get; private set; }

		/// <summary>
		/// Текущее исключение
		/// </summary>
		public ExceptionObject CurrentException { get; private set; }

		/// <summary>
		/// Обработчик необработанных JS исключений
		/// </summary>
		public Action<ExceptionObject> OnUnhandledException { get; set; }

		/// <summary>
		/// Выполнение потока завершено?
		/// </summary>
		public bool IsTerminated { get; private set; }

		/// <summary>
		/// Количество выполненных инструкций
		/// </summary>
		public int ExecutedStepCount { get; private set; }

		/// <summary>
		/// Результат выполнения потока. Доступен только после завершения работы потока
		/// </summary>
		public JSValue Result { get; private set; }
	}
}
