using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using YaJS.Compiler.AST.Expressions;
using YaJS.Runtime;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Представляет в AST дереве выражение. Является базовым классом для всех выражений
	/// </summary>
	public abstract class Expression {
		private static readonly Expression UndefinedLiteral = new UndefinedLiteral();
		private static readonly Expression NullLiteral = new NullLiteral();
		private static readonly Expression FalseLiteral = new BooleanLiteral(false);
		private static readonly Expression TrueLiteral = new BooleanLiteral(true);
		private static readonly Expression EmptyStringLiteral = new StringLiteral(string.Empty);
		private static readonly List<KeyValuePair<string, Expression>> EmptyObjectLiteral =
			new List<KeyValuePair<string, Expression>>();
		private static readonly List<Expression> EmptyArrayLiteral = new List<Expression>();
		private static readonly Expression ThisLiteral = new This();
		private static readonly Expression ArgumentsLiteral = new Arguments();
		private static readonly Expression EvalLiteral = new Eval();

		protected Expression(ExpressionType type) {
			Type = type;
		}

		/// <summary>
		/// Компилировать выражение
		/// </summary>
		/// <param name="compiler">компилятор функции</param>
		/// <param name="isLastOperator">является ли оператор последним в выражении</param>
		internal virtual void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			Contract.Requires(compiler != null);
		}

		public virtual JSValue ToJSValue() {
			throw new NotSupportedException();
		}

		public static Expression Undefined() {
			return (UndefinedLiteral);
		}

		public static Expression Null() {
			return (NullLiteral);
		}

		public static Expression False() {
			return (FalseLiteral);
		}

		public static Expression True() {
			return (TrueLiteral);
		}

		public static Expression Integer(int value) {
			return (new IntegerLiteral(value));
		}

		public static Expression Integer(string value) {
			Contract.Requires(!string.IsNullOrEmpty(value));
			var prevIntegerValue = 0;
			var integerValue = 0;
			for (var i = 0; i < value.Length; i++) {
				var digit = (value[i] - '0');
				Contract.Assert(0 <= digit && digit <= 9);
				integerValue = unchecked(10 * prevIntegerValue + digit);
				if (integerValue < prevIntegerValue) {
					var floatValue = (double)prevIntegerValue;
					for (var j = i; j < value.Length; j++) {
						digit = (value[j] - '0');
						Contract.Assert(0 <= digit && digit <= 9);
						floatValue = 10 * floatValue + digit;
					}
					return (new FloatLiteral(floatValue));
				}
				prevIntegerValue = integerValue;
			}
			return (new IntegerLiteral(integerValue));
		}

		private static int ToHexDigit(char c) {
			if ('0' <= c && c <= '9')
				return (c - '0');
			if ('a' <= c && c <= 'f')
				return (c - 'a') + 10;
			if ('A' <= c && c <= 'F')
				return (c - 'A') + 10;
			Contract.Assert(false);
			return (0);
		}

		public static Expression HexInteger(string value) {
			Contract.Requires(!string.IsNullOrEmpty(value));
			var prevIntegerValue = 0;
			var integerValue = 0;
			for (var i = 0; i < value.Length; i++) {
				var digit = ToHexDigit(value[i]);
				integerValue = unchecked((prevIntegerValue << 4) | digit);
				if (integerValue < prevIntegerValue) {
					var floatValue = (double)prevIntegerValue;
					for (var j = i; j < value.Length; j++) {
						digit = ToHexDigit(value[j]);
						floatValue = 16 * floatValue + digit;
					}
					return (new FloatLiteral(floatValue));
				}
				prevIntegerValue = integerValue;
			}
			return (new IntegerLiteral(integerValue));
		}

		public static Expression Float(double value) {
			return (new FloatLiteral(value));
		}

		public static Expression Float(string value) {
			Contract.Requires(!string.IsNullOrEmpty(value));
			return (new FloatLiteral(double.Parse(value, CultureInfo.InvariantCulture)));
		}

		public static Expression String(string value) {
			if (string.IsNullOrEmpty(value))
				return (EmptyStringLiteral);
			return (new StringLiteral(value));
		}

		public static Expression Ident(string value) {
			return (new Identifier(value));
		}

		public static Expression Object(List<KeyValuePair<string, Expression>> properties) {
			Contract.Requires(properties != null);
			if (properties.Count == 0)
				properties = EmptyObjectLiteral;
			return (new ObjectLiteral(properties));
		}

		public static Expression Array(List<Expression> items) {
			Contract.Requires(items != null);
			if (items.Count == 0)
				items = EmptyArrayLiteral;
			return (new ArrayLiteral(items));
		}

		public static Expression Function(Function function) {
			return (new FunctionLiteral(function));
		}

		public static Expression This() {
			return (ThisLiteral);
		}

		public static Expression Arguments() {
			return (ArgumentsLiteral);
		}

		public static Expression Eval() {
			return (EvalLiteral);
		}

		public static Expression Grouping(Expression operand) {
			return (new GroupingOperator(operand));
		}

		public static Expression Member(Expression baseValue, Expression memberName, bool isPropertyExpression) {
			return (new MemberOperator(baseValue, memberName, isPropertyExpression));
		}

		public static Expression New(Expression constructor, List<Expression> argumentList) {
			return (new NewOperator(constructor, argumentList));
		}

		public static Expression Call(Expression function, List<Expression> argumentList) {
			return (new CallOperator(function, argumentList));
		}

		public static Expression PostfixInc(Expression operand) {
			return (new PostfixIncOperator(operand));
		}

		public static Expression PostfixDec(Expression operand) {
			return (new PostfixDecOperator(operand));
		}

		public static Expression Delete(Expression operand) {
			return (new DeleteOperator(operand));
		}

		public static Expression Void(Expression operand) {
			return (new VoidOperator(operand));
		}

		public static Expression TypeOf(Expression operand) {
			return (new TypeOfOperator(operand));
		}

		public static Expression Inc(Expression operand) {
			return (new IncOperator(operand));
		}

		public static Expression Dec(Expression operand) {
			return (new DecOperator(operand));
		}

		public static Expression Pos(Expression operand) {
			return (new PosOperator(operand));
		}

		public static Expression Neg(Expression operand) {
			return (new NegOperator(operand));
		}

		public static Expression BitNot(Expression operand) {
			return (new BitNotOperator(operand));
		}

		public static Expression Not(Expression operand) {
			return (new NotOperator(operand));
		}

		public static Expression Mul(Expression leftOperand, Expression rightOperand) {
			return (new MulOperator(leftOperand, rightOperand));
		}

		public static Expression Div(Expression leftOperand, Expression rightOperand) {
			return (new DivOperator(leftOperand, rightOperand));
		}

		public static Expression Mod(Expression leftOperand, Expression rightOperand) {
			return (new ModOperator(leftOperand, rightOperand));
		}

		public static Expression Plus(Expression leftOperand, Expression rightOperand) {
			return (new PlusOperator(leftOperand, rightOperand));
		}

		public static Expression Minus(Expression leftOperand, Expression rightOperand) {
			return (new MinusOperator(leftOperand, rightOperand));
		}

		public static Expression Shl(Expression leftOperand, Expression rightOperand) {
			return (new ShlOperator(leftOperand, rightOperand));
		}

		public static Expression ShrS(Expression leftOperand, Expression rightOperand) {
			return (new ShrSOperator(leftOperand, rightOperand));
		}

		public static Expression ShrU(Expression leftOperand, Expression rightOperand) {
			return (new ShrUOperator(leftOperand, rightOperand));
		}

		public static Expression Lt(Expression leftOperand, Expression rightOperand) {
			return (new LtOperator(leftOperand, rightOperand));
		}

		public static Expression Lte(Expression leftOperand, Expression rightOperand) {
			return (new LteOperator(leftOperand, rightOperand));
		}

		public static Expression Gt(Expression leftOperand, Expression rightOperand) {
			return (new GtOperator(leftOperand, rightOperand));
		}

		public static Expression Gte(Expression leftOperand, Expression rightOperand) {
			return (new GteOperator(leftOperand, rightOperand));
		}

		public static Expression InstanceOf(Expression leftOperand, Expression rightOperand) {
			return (new InstanceOfOperator(leftOperand, rightOperand));
		}

		public static Expression In(Expression leftOperand, Expression rightOperand) {
			return (new InOperator(leftOperand, rightOperand));
		}

		public static Expression Eq(Expression leftOperand, Expression rightOperand) {
			return (new EqOperator(leftOperand, rightOperand));
		}

		public static Expression Neq(Expression leftOperand, Expression rightOperand) {
			return (new NeqOperator(leftOperand, rightOperand));
		}

		public static Expression StrictEq(Expression leftOperand, Expression rightOperand) {
			return (new StrictEqOperator(leftOperand, rightOperand));
		}

		public static Expression StrictNeq(Expression leftOperand, Expression rightOperand) {
			return (new StrictNeqOperator(leftOperand, rightOperand));
		}

		public static Expression BitAnd(Expression leftOperand, Expression rightOperand) {
			return (new BitAndOperator(leftOperand, rightOperand));
		}

		public static Expression BitXor(Expression leftOperand, Expression rightOperand) {
			return (new BitXorOperator(leftOperand, rightOperand));
		}

		public static Expression BitOr(Expression leftOperand, Expression rightOperand) {
			return (new BitOrOperator(leftOperand, rightOperand));
		}

		public static Expression And(Expression leftOperand, Expression rightOperand) {
			return (new AndOperator(leftOperand, rightOperand));
		}

		public static Expression Or(Expression leftOperand, Expression rightOperand) {
			return (new OrOperator(leftOperand, rightOperand));
		}

		public static Expression Conditional(Expression condition, Expression trueOperand, Expression falseOperand) {
			return (new ConditionalOperator(condition, trueOperand, falseOperand));
		}

		public static Expression SimpleAssign(Expression lhs, Expression rhs) {
			return (new SimpleAssignOperator(lhs, rhs));
		}

		public static Expression PlusAssign(Expression lhs, Expression rhs) {
			return (new PlusAssignOperator(lhs, rhs));
		}

		public static Expression MinusAssign(Expression lhs, Expression rhs) {
			return (new MinusAssignOperator(lhs, rhs));
		}

		public static Expression MulAssign(Expression lhs, Expression rhs) {
			return (new MulAssignOperator(lhs, rhs));
		}

		public static Expression DivAssign(Expression lhs, Expression rhs) {
			return (new DivAssignOperator(lhs, rhs));
		}

		public static Expression ModAssign(Expression lhs, Expression rhs) {
			return (new ModAssignOperator(lhs, rhs));
		}

		public static Expression ShlAssign(Expression lhs, Expression rhs) {
			return (new ShlAssignOperator(lhs, rhs));
		}

		public static Expression ShrSAssign(Expression lhs, Expression rhs) {
			return (new ShrSAssignOperator(lhs, rhs));
		}

		public static Expression ShrUAssign(Expression lhs, Expression rhs) {
			return (new ShrUAssignOperator(lhs, rhs));
		}

		public static Expression BitAndAssign(Expression lhs, Expression rhs) {
			return (new BitAndAssignOperator(lhs, rhs));
		}

		public static Expression BitXorAssign(Expression lhs, Expression rhs) {
			return (new BitXorAssignOperator(lhs, rhs));
		}

		public static Expression BitOrAssign(Expression lhs, Expression rhs) {
			return (new BitOrAssignOperator(lhs, rhs));
		}

		public static Expression Sequence(List<Expression> operands) {
			return (new SequenceOperator(operands));
		}

		/// <summary>
		/// Тип выражения
		/// </summary>
		public ExpressionType Type { get; private set; }

		/// <summary>
		/// Является ли выражение ссылкой (можно использовать в левой части оператора присваивания)
		/// </summary>
		public virtual bool IsReference { get { return (false); } }

		/// <summary>
		/// Применим ли к выражению MemberOperator
		/// </summary>
		public virtual bool CanHaveMembers { get { return (false); } }

		/// <summary>
		/// Будет ли выражение ссылкой после применения MemberOperator
		/// </summary>
		public virtual bool CanHaveMutableMembers { get { return (false); } }

		/// <summary>
		/// Применим ли к выражению NewOperator
		/// </summary>
		public virtual bool CanBeConstructor { get { return (false); } }

		/// <summary>
		/// Применим ли к выражению CallOperator
		/// </summary>
		public virtual bool CanBeFunction { get { return (false); } }

		/// <summary>
		/// Применим ли к выражению DeleteOperator
		/// </summary>
		public virtual bool CanBeDeleted { get { return (false); } }

		/// <summary>
		/// Может ли результат вычисления выражения быть объектом
		/// </summary>
		public virtual bool CanBeObject { get { return (false); } }

		/// <summary>
		/// Выражение является константным
		/// </summary>
		public virtual bool IsConstant { get { return (false); } }

		/// <summary>
		/// Может ли использоваться в качестве Selector-а в CaseClause
		/// </summary>
		public virtual bool CanBeUsedInCaseClause { get { return (false); } }
	}
}
