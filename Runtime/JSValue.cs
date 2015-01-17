using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Objects;
using YaJS.Runtime.Values;

namespace YaJS.Runtime {
	/// <summary>
	/// Типы примитивных значений
	/// </summary>
	public enum JSValueType {
		Undefined,
		Null,
		Boolean,
		Integer,
		Float,
		String,
		Object,
		Enumerator
	}

	/// <summary>
	/// Примитивное значение
	/// </summary>
	[Serializable]
	public abstract class JSValue {
		public static readonly JSValue Undefined = new JSUndefinedValue();
		public static readonly JSValue Null = new JSNullValue();

		protected JSValue(JSValueType type) {
			Type = type;
		}

		public virtual IEnumerator<JSValue> GetEnumerator() {
			throw new TypeErrorException();
		}

		internal JSEnumerator GetJSEnumerator() {
			return (new JSEnumerator(GetEnumerator()));
		}

		public JSValue Plus(JSValue value) {
			Contract.Requires(value != null);
			if (Type == JSValueType.String || value.Type == JSValueType.String)
				return (CastToString() + CastToString());
			return (ToNumber().Plus(value.ToNumber()));
		}

		public JSValue Not() {
			return (!CastToBoolean());
		}

		public JSValue And(JSValue value) {
			Contract.Requires(value != null);
			return (!CastToBoolean() ? this : value);
		}

		public JSValue Or(JSValue value) {
			Contract.Requires(value != null);
			return (CastToBoolean() ? this : value);
		}

		public virtual bool ConvEqualsTo(JSValue value) {
			Contract.Requires(value != null);
			throw new TypeErrorException();
		}

		public virtual bool StrictEqualsTo(JSValue value) {
			Contract.Requires(value != null);
			throw new TypeErrorException();
		}

		public bool Lt(JSValue value) {
			Contract.Requires(value != null);
			if (Type == JSValueType.String && value.Type == JSValueType.String)
				return (string.CompareOrdinal(CastToString(), value.CastToString()) == -1);
			return (ToNumber().Lt(value.ToNumber()));
		}

		public bool Lte(JSValue value) {
			Contract.Requires(value != null);
			if (Type == JSValueType.String && value.Type == JSValueType.String)
				return (string.CompareOrdinal(CastToString(), value.CastToString()) <= 0);
			return (ToNumber().Lte(value.ToNumber()));
		}

		public virtual bool IsInstanceOf(JSFunction constructor) {
			Contract.Requires(constructor != null);
			throw new TypeErrorException();
		}

		public virtual string TypeOf() {
			throw new TypeErrorException();
		}

		public virtual bool CastToBoolean() {
			throw new TypeErrorException();
		}

		public virtual int CastToInteger() {
			throw new TypeErrorException();
		}

		public virtual double CastToFloat() {
			throw new TypeErrorException();
		}

		public virtual string CastToString() {
			throw new TypeErrorException();
		}

		public virtual int RequireInteger() {
			throw new TypeErrorException();
		}

		public virtual JSObject RequireObject() {
			throw new TypeErrorException();
		}

		public virtual JSFunction RequireFunction() {
			throw new TypeErrorException();
		}

		internal virtual JSEnumerator RequireEnumerator() {
			throw new TypeErrorException();
		}

		public virtual JSNumberValue ToNumber() {
			throw new TypeErrorException();
		}

		public virtual void CastToPrimitiveValue(ExecutionThread thread, Action<JSValue> onCompleteCallback) {
			Contract.Requires(thread != null);
			Contract.Requires(onCompleteCallback != null);
			throw new NotSupportedException();
		}

		public virtual JSObject ToObject(VirtualMachine vm) {
			Contract.Requires(vm != null);
			throw new TypeErrorException();
		}

		public static implicit operator JSValue(bool value) {
			return (new JSBooleanValue(value));
		}

		public static implicit operator JSValue(string value) {
			return (new JSStringValue(value));
		}

		public JSValueType Type { get; private set; }
	}
}
