using System;

namespace YaJS.Runtime {
	using Runtime.Exceptions;
	using Runtime.Objects;
	using Runtime.Values;

	/// <summary>
	/// Типы примитивных значений
	/// </summary>
	public enum JSValueType { Undefined, Null, Boolean, Integer, Float, String, Object }

	/// <summary>
	/// Примитивное значение
	/// </summary>
	[Serializable]
	public abstract class JSValue : IComparable<JSValue> {
		public static readonly JSUndefinedValue Undefined = new JSUndefinedValue();
		public static readonly JSNullValue Null = new JSNullValue();

		public JSValue(JSValueType type) {
			Type = type;
		}

		public virtual bool ContainsMember(JSValue name) {
			throw new TypeErrorException();
		}

		public virtual JSValue GetMember(VirtualMachine runtime, JSValue name) {
			throw new TypeErrorException();
		}

		public virtual void SetMember(VirtualMachine runtime, JSValue name, JSValue value) {
			throw new TypeErrorException();
		}

		public virtual bool DeleteMember(JSValue name) {
			throw new TypeErrorException();
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
			return (JSValue.Create(!CastToBoolean()));
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

		public virtual int CompareTo(JSValue value) {
			throw new TypeErrorException();
		}

		public virtual bool IsInstanceOf(JSFunction constructor) {
			throw new TypeErrorException();
		}

		public virtual string TypeOf() {
			throw new TypeErrorException();
		}

		public virtual bool CastToBoolean() {
			throw new TypeErrorException();
		}
		public virtual double CastToInteger() {
			throw new TypeErrorException();
		}
		public virtual double CastToFloat() {
			throw new TypeErrorException();
		}
		public virtual string CastToString() {
			throw new TypeErrorException();
		}

		public virtual bool GetAsBoolean() {
			throw new TypeErrorException();
		}
		public virtual int GetAsInteger() {
			throw new TypeErrorException();
		}
		public virtual double GetAsFloat() {
			throw new TypeErrorException();
		}
		public virtual string GetAsString() {
			throw new TypeErrorException();
		}
		public virtual JSObject GetAsObject() {
			throw new TypeErrorException();
		}
		public virtual JSFunction GetAsFunction() {
			throw new TypeErrorException();
		}

		public virtual JSObject ToObject(VirtualMachine vm) {
			throw new NotImplementedException();
		}
		public virtual JSValue ValueOf() {
			return (this);
		}

		public static JSValue ToPrimitiveValue(JSValue value) {
			if (value.Type == JSValueType.Object)
				value = value.ValueOf();
			return (value);
		}

		public static JSValue Create(bool value) {
			return (new JSBooleanValue(value));
		}

		public static JSValue Create(int value) {
			return (new JSIntegerValue(value));
		}

		public static JSValue Create(double value) {
			return (new JSFloatValue(value));
		}

		public static JSValue Create(string value) {
			return (new JSStringValue(value));
		}

		public JSValueType Type { get; private set; }
	}
}
