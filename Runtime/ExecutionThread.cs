using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	using Runtime.Exceptions;
	using Runtime.Objects;
	using Runtime.Objects.Errors;

	/// <summary>
	/// Поток исполнения в рамках которого исполняется JS-функция
	/// </summary>
	public sealed class ExecutionThread {
		internal ExecutionThread(VirtualMachine vm, CompiledFunction mainFunction) {
			Contract.Requires(vm != null);
			Contract.Requires(mainFunction != null);
			VM = vm;
			MainFunction = new JSManagedFunction(mainFunction, null, VM.Function);
			CurrentFrame = new CallStackFrame(VM, MainFunction, vm.Global, new List<JSValue>(), null, false);
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

		private void Throw(JSValue thrownValue) {
			if (CurrentException == null || !Object.ReferenceEquals(CurrentException.ThrownValue, thrownValue)) {
				CurrentException = new ExceptionObject(thrownValue, CurrentException);
			}
			Unwind();
		}

		private void NewObject(JSFunction constructor, List<JSValue> args) {
			if (constructor.IsNative) {
				CurrentFrame.Push(constructor.Invoke(
					VM, null, CurrentFrame.LocalScope, args
				));
			}
			else {
				var newObject = new JSObject(constructor.GetPrototype(VM));
				CurrentFrame = new CallStackFrame(
					VM, constructor as JSManagedFunction, newObject, args, CurrentFrame, true
				);
				CurrentFrame.Push(newObject);
			}
		}

		private void MakeObject(int memberCount) {
			var newObject = VM.NewObject();
			for (var i = memberCount - 1; i >= 0; i--) {
				var memberName = CurrentFrame.Pop().CastToString();
				var memberValue = CurrentFrame.Pop();
				newObject.OwnMembers.Add(memberName, memberValue);
			}
			CurrentFrame.Push(newObject);
		}

		private void MakeArray(int memberCount) {
			var newArray = VM.NewArray(new List<JSValue>(memberCount));
			for (var i = memberCount - 1; i >= 0; i--) {
				newArray.Items[i] = CurrentFrame.Pop();
			}
			CurrentFrame.Push(newArray);
		}

		private void CallFunction(
			JSFunction function, JSObject context, List<JSValue> args, bool copyResult
		) {
			if (function.IsNative) {
				CurrentFrame.Push(function.Invoke(
					VM, context, CurrentFrame.LocalScope, args
				));
			}
			else {
				CurrentFrame = new CallStackFrame(
					VM, function as JSManagedFunction, context, args, CurrentFrame, copyResult
				);
			}
		}

		#region ExecuteStep

		/// <summary>
		/// Выполнить следующую инструкцию
		/// </summary>
		/// <returns>False - если выполненная инструкция является последней, иначе true</returns>
		public bool ExecuteStep() {
			if (IsTerminated)
				throw new InvalidThreadStateException();

			var currentFrame = CurrentFrame;

			try {
				try {
					switch (currentFrame.CodeReader.ReadOpCode()) {
						case OpCode.Nop:
							break;

						case OpCode.LdUndefined:
							currentFrame.Push(JSValue.Undefined);
							break;
						case OpCode.LdNull:
							currentFrame.Push(JSValue.Null);
							break;
						case OpCode.LdBoolean:
							currentFrame.Push(JSValue.Create(currentFrame.CodeReader.ReadBoolean()));
							break;
						case OpCode.LdInteger:
							currentFrame.Push(JSValue.Create(currentFrame.CodeReader.ReadInteger()));
							break;
						case OpCode.LdFloat:
							currentFrame.Push(JSValue.Create(currentFrame.CodeReader.ReadFloat()));
							break;
						case OpCode.LdString:
							currentFrame.Push(JSValue.Create(currentFrame.CodeReader.ReadString()));
							break;

						case OpCode.LdThis:
							currentFrame.Push(CurrentFrame.Context);
							break;

						case OpCode.DeclLocal:
							currentFrame.LocalScope.DeclareLocal(currentFrame.CodeReader.ReadString());
							break;

						case OpCode.LdLocal:
							currentFrame.Push(
								currentFrame.LocalScope.GetLocalVariable(this, currentFrame.CodeReader.ReadString())
							);
							break;
						case OpCode.LdLocalFunc:
							currentFrame.Push(
								currentFrame.GetLocalFunction(VM, currentFrame.CodeReader.ReadInteger())
							);
							break;
						case OpCode.StLocal:
							currentFrame.LocalScope.SetLocalVariable(
								this, currentFrame.CodeReader.ReadString(), currentFrame.Pop()
							);
							break;

						case OpCode.IsMember: {
								var obj = currentFrame.Pop().GetAsObject();
								var memberName = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(obj.ContainsMember(memberName)));
								break;
							}
						case OpCode.LdMember: {
								var obj = currentFrame.Pop().GetAsObject();
								var memberName = currentFrame.PopPrimitiveValue();
								currentFrame.Push(obj.GetMember(VM, memberName));
								break;
							}
						case OpCode.StMember: {
								var obj = currentFrame.Pop().GetAsObject();
								var memberName = currentFrame.PopPrimitiveValue();
								obj.SetMember(VM, memberName, currentFrame.Pop());
								break;
							}
						case OpCode.DelMember: {
								var obj = currentFrame.Pop().GetAsObject();
								var memberName = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(obj.DeleteMember(memberName)));
								break;
							}

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

						case OpCode.BeginScope:
							currentFrame.BeginScope();
							break;
						case OpCode.EndScope:
							currentFrame.EndScope();
							break;

						case OpCode.BeginTry:
							currentFrame.BeginTry();
							break;
						case OpCode.EndTry:
							currentFrame.EndTry();
							break;

						case OpCode.EndCatch:
							CurrentException = null;
							break;

						case OpCode.EndFinally:
							if (CurrentException != null)
								Unwind();
							break;

						case OpCode.Throw:
							Throw(currentFrame.Pop());
							break;

						case OpCode.NewObj:
							var constructor = currentFrame.Pop().GetAsFunction();
							NewObject(constructor, currentFrame.PopArguments());
							break;

						case OpCode.MakeObject:
							MakeObject(currentFrame.Pop().GetAsInteger());
							break;
						case OpCode.MakeArray:
							MakeArray(currentFrame.Pop().GetAsInteger());
							break;

						case OpCode.Call: {
								var function = currentFrame.Pop().GetAsFunction();
								CallFunction(
									function, VM.Global, currentFrame.PopArguments(), currentFrame.CodeReader.ReadBoolean()
								);
								break;
							}

						case OpCode.CallMember: {
								var function = currentFrame.Pop().GetAsFunction();
								CallFunction(
									function, currentFrame.Pop().GetAsObject(), currentFrame.PopArguments(), currentFrame.CodeReader.ReadBoolean()
								);
								break;
							}

						case OpCode.Return:
							if (currentFrame.CopyResult) {
								Contract.Assert(currentFrame.Caller != null);
								CurrentFrame.Caller.Push(currentFrame.Pop());
							}
							CurrentFrame = currentFrame.Caller;
							break;

						case OpCode.Neg:
							currentFrame.Push(currentFrame.PopPrimitiveValue().Neg());
							break;

						case OpCode.Plus: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.Plus(op2));
								break;
							}
						case OpCode.Minus: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.Minus(op2));
								break;
							}
						case OpCode.Mul: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.Mul(op2));
								break;
							}
						case OpCode.Div: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.Div(op2));
								break;
							}
						case OpCode.Mod: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.Mod(op2));
								break;
							}

						case OpCode.BitNot:
							currentFrame.Push(currentFrame.PopPrimitiveValue().BitNot());
							break;
						case OpCode.BitAnd: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.BitAnd(op2));
								break;
							}
						case OpCode.BitOr: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.BitOr(op2));
								break;
							}
						case OpCode.BitXor: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.BitXor(op2));
								break;
							}

						case OpCode.Shl: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.BitShl(op2));
								break;
							}
						case OpCode.ShrS: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.BitShrS(op2));
								break;
							}
						case OpCode.ShrU: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(op1.BitShrU(op2));
								break;
							}

						case OpCode.Not:
							currentFrame.Push(JSValue.Create(!currentFrame.Pop().CastToBoolean()));
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

						case OpCode.Eq: {
								var op1 = currentFrame.Pop();
								var op2 = currentFrame.Pop();
								currentFrame.Push(JSValue.Create(op1.EqualsTo(op2)));
								break;
							}
						case OpCode.StrictEq: {
								var op1 = currentFrame.Pop();
								var op2 = currentFrame.Pop();
								currentFrame.Push(JSValue.Create(op1.Type == op2.Type && op1.StrictEqualsTo(op2)));
								break;
							}
						case OpCode.Neq: {
								var op1 = currentFrame.Pop();
								var op2 = currentFrame.Pop();
								currentFrame.Push(JSValue.Create(!op1.EqualsTo(op2)));
								break;
							}
						case OpCode.StrictNeq: {
								var op1 = currentFrame.Pop();
								var op2 = currentFrame.Pop();
								currentFrame.Push(JSValue.Create(op1.Type != op2.Type || !op1.StrictEqualsTo(op2)));
								break;
							}

						case OpCode.Cmp: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(op1.CompareTo(op2)));
								break;
							}

						case OpCode.Gt: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(op1.CompareTo(op2) > 0));
								break;
							}
						case OpCode.Gte: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(op1.CompareTo(op2) >= 0));
								break;
							}
						case OpCode.Lt: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(op1.CompareTo(op2) < 0));
								break;
							}
						case OpCode.Lte: {
								var op1 = currentFrame.PopPrimitiveValue();
								var op2 = currentFrame.PopPrimitiveValue();
								currentFrame.Push(JSValue.Create(op1.CompareTo(op2) <= 0));
								break;
							}

						case OpCode.InstanceOf: {
								var op1 = currentFrame.Pop().GetAsObject();
								var op2 = currentFrame.Pop().GetAsFunction();
								currentFrame.Push(JSValue.Create(op1.IsInstanceOf(op2)));
								break;
							}
						case OpCode.TypeOf:
							currentFrame.Push(JSValue.Create(currentFrame.Pop().TypeOf()));
							break;

						default:
							throw new UnknownOpCodeException();
					}

					IsTerminated = CurrentFrame == null;
					if (IsTerminated)
						Result = currentFrame.Pop();

					ExecutedStepCount++;
				}
				catch (RuntimeErrorException ex) {
					Throw(VM.NewInternalError(ex.Message, ex.StackTrace));
				}
			}
			catch (Exception ex) {
				// Произошла серьезная ошибка связанная с внутренним состоянием потока.
				// Поток должен быть завершен. Дальнейшее его использование невозможно
				IsTerminated = true;
				throw new UnrecoverableErrorException(
					ex.Message, currentFrame.ToStackTrace(), ex
				);
			}

			return (IsTerminated);
		}

		#endregion

		/// <summary>
		/// Виртуальная машина
		/// </summary>
		public VirtualMachine VM { get; private set; }
		/// <summary>
		/// Исполняемая функция
		/// </summary>
		public JSManagedFunction MainFunction { get; private set; }
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
