using System;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Objects;

namespace YaJS.Runtime.Values {
	/// <summary>
	/// Базовый класс для числовых значений
	/// </summary>
	[Serializable]
	public abstract class JSNumberValue : JSValue {
		protected JSNumberValue(JSValueType type)
			: base(type) {
		}

		public abstract JSNumberValue Neg();
		public abstract JSNumberValue Inc();
		public abstract JSNumberValue Dec();

		public virtual JSNumberValue Plus(JSNumberValue value) {
			Contract.Requires(value != null);
			throw new NotSupportedException();
		}

		public virtual JSNumberValue Minus(JSNumberValue value) {
			Contract.Requires(value != null);
			throw new NotSupportedException();
		}

		public virtual JSNumberValue Mul(JSNumberValue value) {
			Contract.Requires(value != null);
			throw new NotSupportedException();
		}

		public JSNumberValue IntDiv(JSNumberValue value) {
			Contract.Requires(value != null);
			Contract.Ensures(Contract.Result<JSNumberValue>().Type == JSValueType.Integer);
			return (CastToInteger() / value.CastToInteger());
		}

		public JSNumberValue FltDiv(JSNumberValue value) {
			Contract.Requires(value != null);
			return (CastToFloat() / value.CastToFloat());
		}

		public virtual JSNumberValue Mod(JSNumberValue value) {
			Contract.Requires(value != null);
			throw new NotSupportedException();
		}

		public JSNumberValue BitNot() {
			return (~CastToInteger());
		}

		public JSNumberValue BitAnd(JSNumberValue value) {
			Contract.Requires(value != null);
			return (CastToInteger() & value.CastToInteger());
		}

		public JSNumberValue BitOr(JSNumberValue value) {
			Contract.Requires(value != null);
			return (CastToInteger() | value.CastToInteger());
		}

		public JSNumberValue BitXor(JSNumberValue value) {
			Contract.Requires(value != null);
			return (CastToInteger() ^ value.CastToInteger());
		}

		public JSNumberValue BitShl(JSNumberValue value) {
			Contract.Requires(value != null);
			return (CastToInteger() << value.CastToInteger());
		}

		public JSNumberValue BitShrS(JSNumberValue value) {
			Contract.Requires(value != null);
			return (CastToInteger() >> value.CastToInteger());
		}

		public JSNumberValue BitShrU(JSNumberValue value) {
			Contract.Requires(value != null);
			return ((int)((uint)CastToInteger() >> value.CastToInteger()));
		}

		public abstract bool Lt(JSNumberValue value);
		public abstract bool Lte(JSNumberValue value);

		public override string TypeOf() {
			return ("number");
		}

		public override JSNumberValue ToNumber() {
			return (this);
		}

		public override JSObject ToObject(VirtualMachine vm) {
			return (vm.NewNumber(this));
		}

		public static implicit operator JSNumberValue(int value) {
			return (new JSIntegerValue(value));
		}

		public static implicit operator JSNumberValue(double value) {
			return (new JSFloatValue(value));
		}

		public static int ParseInteger(string value) {
			Contract.Requires(value != null);
			throw new NotImplementedException();
		}

		public static double ParseFloat(string value) {
			Contract.Requires(value != null);
			throw new NotImplementedException();
		}

		public static JSNumberValue ParseNumber(string value) {
			Contract.Requires(value != null);
			throw new NotImplementedException();
		}
	}
}
