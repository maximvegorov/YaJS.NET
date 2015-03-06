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

		private struct NumberParser {
			private static readonly char[] InfinityLiteral = { 'I', 'n', 'f', 'i', 'n', 'i', 't', 'y' };

			private readonly string _source;
			private int _index;
			private char _curChar;

			public NumberParser(string source) {
				Contract.Requires(source != null);
				_source = source;
				_index = 0;
				_curChar = '\0';
				ReadChar();
			}

			private void ReadChar() {
				_curChar = _index < _source.Length ? _source[_index++] : '\0';
			}

			private bool TryParseDecimalLiteralExponent(out int result) {
				const int maxExponent = 308;
				var neg = false;
				if (_curChar == '+' || _curChar == '-') {
					neg = _curChar == '-';
					ReadChar();
				}
				result = 0;
				if (!char.IsDigit(_curChar))
					return (false);
				do {
					var digit = (_curChar - '0');
					result = 10 * result + digit;
					if (result > maxExponent)
						return (false);
					ReadChar();
				} while (char.IsDigit(_curChar));
				if (neg)
					result = -result;
				return (true);
			}

			private bool IsEndOrRemainWhiteSpaces() {
				while (_curChar != 0) {
					if (!char.IsWhiteSpace(_curChar))
						return (false);
					ReadChar();
				}
				return (true);
			}

			private JSNumberValue ParseDecimalLiteral(bool neg) {
				if (!char.IsDigit(_curChar) && _curChar != '.')
					return (double.NaN);

				var prevIntegerValue = 0;
				var integerValue = 0;
				double floatValue;

				if (_curChar == '.')
					floatValue = 0;
				else {
					while (char.IsDigit(_curChar)) {
						var digit = (_curChar - '0');
						integerValue = unchecked(10 * prevIntegerValue + digit);
						if (integerValue < prevIntegerValue)
							break;
						prevIntegerValue = integerValue;
						ReadChar();
					}

					if (_curChar == 0 || char.IsWhiteSpace(_curChar)) {
						if (_curChar != 0) {
							ReadChar();
							if (!IsEndOrRemainWhiteSpaces())
								return (double.NaN);
						}
						return (neg ? -integerValue : integerValue);
					}

					floatValue = prevIntegerValue;
					while (char.IsDigit(_curChar)) {
						var digit = (_curChar - '0');
						floatValue = 10 * floatValue + digit;
						ReadChar();
					}
				}

				var exponentDelta = 0;
				if (_curChar == '.') {
					ReadChar();
					while (char.IsDigit(_curChar)) {
						var digit = (_curChar - '0');
						floatValue = 10 * floatValue + digit;
						exponentDelta--;
						ReadChar();
					}
				}

				int exponent;
				if (_curChar == 'e' || _curChar == 'E') {
					ReadChar();
					if (!TryParseDecimalLiteralExponent(out exponent))
						return (double.NaN);
				}
				else
					exponent = 0;

				exponent += exponentDelta;
				if (exponent != 0)
					floatValue *= Math.Pow(10, exponent);

				if (!IsEndOrRemainWhiteSpaces())
					return (double.NaN);

				return (neg ? -floatValue : floatValue);
			}

			private static int ToHexDigit(char c) {
				if ('0' <= c && c <= '9')
					return (c - '0');
				if ('a' <= c && c <= 'f')
					return (c - 'a') + 10;
				if ('A' <= c && c <= 'F')
					return (c - 'A') + 10;
				return (-1);
			}

			private JSNumberValue ParseHexIntegerLiteral() {
				var prevIntegerValue = 0;
				var integerValue = 0;
				while (_curChar != 0) {
					var digit = ToHexDigit(_curChar);
					if (digit < 0)
						break;
					integerValue = unchecked((prevIntegerValue << 4) | digit);
					if (integerValue < prevIntegerValue) {
						var floatValue = (double)prevIntegerValue;
						while (_curChar != 0) {
							digit = ToHexDigit(_curChar);
							if (digit < 0)
								break;
							floatValue = 16 * floatValue + digit;
							ReadChar();
						}
						if (!IsEndOrRemainWhiteSpaces())
							return (double.NaN);
						return (floatValue);
					}
					prevIntegerValue = integerValue;
					ReadChar();
				}
				if (!IsEndOrRemainWhiteSpaces())
					return (double.NaN);
				return (integerValue);
			}

			private JSNumberValue ParseInfinityLiteral(bool neg) {
				var i = 1;
				while (_curChar != 0 && i < InfinityLiteral.Length) {
					if (_curChar != InfinityLiteral[i])
						return (double.NaN);
					ReadChar();
					i++;
				}
				if (!IsEndOrRemainWhiteSpaces())
					return (double.NaN);
				return (neg ? double.NegativeInfinity : double.PositiveInfinity);
			}

			public JSNumberValue Parse() {
				if (_curChar == 0)
					return (0);
				while (char.IsWhiteSpace(_curChar))
					ReadChar();
				var neg = false;
				switch (_curChar) {
					case '0':
						ReadChar();
						if (_curChar == 'x' || _curChar == 'X') {
							ReadChar();
							return (ParseHexIntegerLiteral());
						}
						break;
					case '+':
					case '-':
						neg = _curChar == '-';
						ReadChar();
						break;
				}
				if (_curChar != 'I')
					return (ParseDecimalLiteral(neg));
				ReadChar();
				return (ParseInfinityLiteral(neg));					
			}
		}

		public static JSNumberValue ParseNumber(string value) {
			Contract.Requires(value != null);
			var parser = new NumberParser(value);
			return (parser.Parse());
		}
	}
}
