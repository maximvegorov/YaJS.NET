using System;

namespace YaJS.Runtime {
	using Runtime.Exceptions;
	using Runtime.Objects;
	using Runtime.Values;

	/// <summary>
	/// Типы примитивных значений
	/// </summary>
	public enum JSValueType { Undefined, Null, Boolean, Integer, Float, String, Object, Enumerator }

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

		public virtual JSEnumerator GetEnumerator() {
			return (JSEnumerator.Empty);
		}

		public virtual JSValue Neg() {
			throw new TypeErrorException();
		}

		public virtual JSValue Plus(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue Minus(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue Mul(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue Div(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue Mod(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue BitNot() {
			throw new TypeErrorException();
		}

		public virtual JSValue BitAnd(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue BitOr(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue BitXor(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue BitShl(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue BitShrS(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual JSValue BitShrU(JSValue value) {
			throw new TypeErrorException();
		}

		public JSValue Not() {
			return (!CastToBoolean());
		}

		public JSValue And(JSValue value) {
			return (!CastToBoolean() ? this : value);
		}

		public JSValue Or(JSValue value) {
			return (CastToBoolean() ? this : value);
		}

		public virtual bool EqualsTo(JSValue value) {
			return (false);
		}

		public virtual bool StrictEqualsTo(JSValue value) {
			return (false);
		}

		public virtual bool Lt(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual bool Lte(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual bool IsInstanceOf(JSFunction constructor) {
			return (false);
		}

		public abstract string TypeOf();

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
		public virtual JSEnumerator RequireEnumerator() {
			throw new TypeErrorException();
		}

		public virtual JSValue ToPrimitiveValue() {
			return (this);
		}

		public virtual JSNumberValue ToNumber() {
			throw new TypeErrorException();
		}

		public virtual JSObject ToObject(VirtualMachine vm) {
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
