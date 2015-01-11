using System.Collections.Generic;
using System.Linq;

namespace YaJS.Compiler {
	using AST;

	/// <summary>
	/// Синтаксис выражений:
	/// Arguments ::= ([Expression(, Expression)*])
	/// PrimaryExpression ::= undefined | null | false | true | Number | String |
	///		Identifier | ObjectLiteral | ArrayLiteral | FunctionLiteral | this |
	///		arguments | eval | (Expression)
	/// MemberExpression ::= PrimaryExpression ([Expression] | .Identifier)* |
	///		new MemberExpression Arguments ([Expression] | .Identifier)*
	/// LeftHandSideExpression ::= new+ MemberExpression | MemberExpression (Arguments | [Expression] | .Identifier)*
	/// PostfixExpression ::= LeftHandSideExpression | LeftHandSideExpression++ | LeftHandSideExpression--
	/// UnaryExpression ::= PostfixExpression | delete UnaryExpression |
	///		void UnaryExpression | typeof UnaryExpression |
	///		++UnaryExpression | --UnaryExpression |
	///		+UnaryExpression | -UnaryExpression |
	///		~UnaryExpression | !UnaryExpression
	/// MultiplicativeExpression ::= UnaryExpression ((* | / | %) UnaryExpression)*
	/// AdditiveExpression ::= MultiplicativeExpression (( + | - ) MultiplicativeExpression)*
	/// ShiftExpression ::= AdditiveExpression (( &lt;&lt; | &gt;&gt; | &gt;&gt;&gt; ) AdditiveExpression)*
	/// RelationalExpression ::= ShiftExpression (( &lt; | &gt; | &lt;= | &gt;= | instanceof | in) ShiftExpression)*
	/// RelationalExpressionNoIn ::= ShiftExpression (( &lt; | &gt; | &lt;= | &gt;= | instanceof) ShiftExpression)*
	/// EqualityExpression ::= RelationalExpression ((== | != | === | !==) RelationalExpression)*
	/// EqualityExpressionNoIn ::= RelationalExpressionNoIn ((== | != | === | !==) RelationalExpressionNoIn)*
	/// BitwiseAndExpression ::= EqualityExpression (& EqualityExpression)*
	/// BitwiseAndExpressionNoIn ::= EqualityExpressionNoIn (& EqualityExpressionNoIn)*
	/// BitwiseXorExpression ::= BitwiseAndExpression (^ BitwiseAndExpression)*
	/// BitwiseXorExpressionNoIn ::= BitwiseAndExpressionNoIn (^ BitwiseAndExpressionNoIn)*
	/// BitwiseOrExpression ::= BitwiseXorExpression (| BitwiseXorExpression)*
	/// BitwiseOrExpressionNoIn ::= BitwiseXorExpressionNoIn (| BitwiseXorExpressionNoIn)*
	/// LogicalAndExpression ::= BitwiseOrExpression (&& BitwiseOrExpression)*
	/// LogicalAndExpressionNoIn ::= BitwiseOrExpressionNoIn (&& BitwiseOrExpressionNoIn)*
	/// LogicalOrExpression ::= LogicalAndExpression (|| LogicalAndExpression)*
	/// LogicalOrExpressionNoIn ::= LogicalAndExpressionNoIn (|| LogicalAndExpressionNoIn)*
	/// ConditionalExpression ::= LogicalOrExpression [? AssignmentExpression : AssignmentExpression]
	/// ConditionalExpressionNoIn ::= LogicalOrExpressionNoIn [? AssignmentExpression : AssignmentExpressionNoIn]
	/// AssignmentExpression ::= ConditionalExpression |
	///		LeftHandSideExpression (= | */ | /= | %= | += | -= | &lt;&lt;= | &gt;&gt;= | &gt;&gt;&gt;= | &= | ^= | |=) AssignmentExpression
	/// AssignmentExpressionNoIn ::= ConditionalExpressionNoIn |
	///		LeftHandSideExpression (= | */ | /= | %= | += | -= | &lt;&lt;= | &gt;&gt;= | &gt;&gt;&gt;= | &= | ^= | |=) AssignmentExpressionNoIn
	/// Expression ::= AssignmentExpression (, AssignmentExpression)*
	/// ExpressionNoIn ::= AssignmentExpressionNoIn (, AssignmentExpressionNoIn)*
	/// </summary>
	public partial class Parser {
		private static readonly List<Expression> EmptyArgumentList = new List<Expression>();

		private List<Expression> ParseArguments() {
			Match(TokenType.LParenthesis);
			if (_lookahead.Type == TokenType.RParenthesis) {
				ReadNextToken();
				return (EmptyArgumentList);
			}
			else {
				var result = new List<Expression>() { ParseExpression() };
				while (_lookahead.Type == TokenType.Comma) {
					ReadNextToken();
					result.Add(ParseExpression());
				}
				Match(TokenType.RParenthesis);
				return (result);
			}
		}

		private IEnumerable<KeyValuePair<string, Expression>> ParseObjectProperties() {
			do {
				ReadNextToken();
				switch (_lookahead.Type) {
					case TokenType.Ident:
					case TokenType.Integer:
					case TokenType.String:
						var name = _lookahead.Value;
						ReadNextToken();
						var value = ParseAssignmentExpression();
						yield return new KeyValuePair<string, Expression>(name, value);
						break;
					case TokenType.RCurlyBrace:
						yield break;
					default:
						throw InvalidToken();
				}
			} while (_lookahead.Type == TokenType.Comma);
		}

		private Expression ParseObject() {
			var properties = ParseObjectProperties().ToList();
			Match(TokenType.RCurlyBrace);
			return (Expression.Object(properties));
		}

		private IEnumerable<Expression> ParseArrayItems() {
			do {
				ReadNextToken();
				if (_lookahead.Type == TokenType.RBracket)
					yield break;
				while (_lookahead.Type == TokenType.Comma) {
					ReadNextToken();
					yield return Expression.Undefined();
				}
				yield return ParseAssignmentExpression();
			} while (_lookahead.Type == TokenType.Comma);
		}

		private Expression ParseArray() {
			var items = ParseArrayItems().ToList();
			Match(TokenType.RBracket);
			return (Expression.Array(items));
		}

		private Expression ParseFunctionLiteral() {
			return (Expression.Function(ParseFunction(false)));
		}

		private Expression ParseGrouping() {
			Match(TokenType.LParenthesis);
			var result = Expression.Grouping(
				ParseExpression()
			);
			Match(TokenType.RParenthesis);
			return (result);
		}

		private Expression ParsePrimaryExpression() {
			switch (_lookahead.Type) {
				case TokenType.Undefined:
					ReadNextToken();
					return (Expression.Undefined());
				case TokenType.Null:
					ReadNextToken();
					return (Expression.Null());
				case TokenType.Integer:
					var integerValue = _lookahead.Value;
					ReadNextToken();
					return (Expression.Integer(integerValue));
				case TokenType.HexInteger:
					var hexIntegerValue = _lookahead.Value;
					ReadNextToken();
					return (Expression.HexInteger(hexIntegerValue));
				case TokenType.Float:
					var floatValue = _lookahead.Value;
					ReadNextToken();
					return (Expression.Float(floatValue));
				case TokenType.String:
					var stringValue = _lookahead.Value;
					ReadNextToken();
					return (Expression.String(stringValue));
				case TokenType.Ident:
					var identValue = _lookahead.Value;
					ReadNextToken();
					return (Expression.Ident(identValue));
				case TokenType.LCurlyBrace:
					// ReadNextToken вызовет ParseObject
					return (ParseObject());
				case TokenType.LBracket:
					// ReadNextToken вызовет ParseArray
					return (ParseArray());
				case TokenType.Function:
					// ReadNextToken вызовет ParseFunctionLiteral
					return (ParseFunctionLiteral());
				case TokenType.This:
					ReadNextToken();
					return (Expression.This());
				case TokenType.Arguments:
					ReadNextToken();
					return (Expression.Arguments());
				case TokenType.Eval:
					ReadNextToken();
					return (Expression.Eval());
				case TokenType.LParenthesis:
					// ReadNextToken вызовет ParseGrouping
					return (ParseGrouping());
				default:
					throw InvalidToken();
			}
		}

		private Expression EatMemberOperators(Expression baseValue) {
			var result = baseValue;
			while (_lookahead.Type == TokenType.LBracket || _lookahead.Type == TokenType.Dot) {
				if (!result.CanHaveMembers)
					ThrowExpectedObject(_lookahead.StartPosition);
				var isLBracket = _lookahead.Type == TokenType.LBracket;
				ReadNextToken();
				if (isLBracket) {
					result = Expression.Member(result, ParseExpression());
					Match(TokenType.RBracket);
				}
				else {
					if (_lookahead.Type != TokenType.Ident)
						ThrowUnmatchedToken(TokenType.Ident, _lookahead);
					result = Expression.Member(result, Expression.Ident(_lookahead.Value));
					ReadNextToken();
				}
			}
			return (result);
		}

		private Expression ParseMemberExpression() {
			Expression baseValue;
			if (_lookahead.Type != TokenType.New) {
				baseValue = ParsePrimaryExpression();
			}
			else {
				ReadNextToken();
				var startPos = _lookahead.StartPosition;
				var constructor = ParseMemberExpression();
				if (!constructor.CanBeConstructor)
					ThrowExpectedFunction(startPos);
				var arguments = ParseArguments();
				baseValue = Expression.New(constructor, arguments);
			}
			return (EatMemberOperators(baseValue));
		}

		private Expression EatCallOrMemberOperators(Expression functionOrBaseValue) {
			var result = functionOrBaseValue;
			while (_lookahead.Type == TokenType.LParenthesis || _lookahead.Type == TokenType.LBracket || _lookahead.Type == TokenType.Dot) {
				if (_lookahead.Type == TokenType.LParenthesis) {
					if (!result.CanBeFunction)
						ThrowExpectedFunction(_lookahead.StartPosition);
					var arguments = ParseArguments();
					result = Expression.Call(result, arguments);
				}
				else {
					if (!result.CanHaveMembers)
						ThrowExpectedObject(_lookahead.StartPosition);
					var isLBracket = _lookahead.Type == TokenType.LBracket;
					ReadNextToken();
					if (isLBracket) {
						result = Expression.Member(result, ParseExpression());
						Match(TokenType.RBracket);
					}
					else {
						if (_lookahead.Type != TokenType.Ident)
							ThrowUnmatchedToken(TokenType.Ident, _lookahead);
						result = Expression.Member(result, Expression.Ident(_lookahead.Value));
						ReadNextToken();
					}
				}
			}
			return (result);
		}

		private Expression ParseLeftHandSideExpression() {
			if (_lookahead.Type != TokenType.New) {
				return (EatCallOrMemberOperators(ParseMemberExpression()));
			}

			// Теперь необходимо выбрать между NewExpression и MemberExpression
			// (Cм. http://www.ecma-international.org/ecma-262/5.1/#sec-11.2)
			// Для этого считаем кол-во последовательно идущих операторов new и
			// для каждого из них пытаемся опредить относится ли он к MemberExpression.
			// Как только мы встречаем первый который не относится к MemberExpression,
			// значит все последующие также относятся к NewExpression
			var newOperatorStack = new Stack<TokenPosition>();
			do {
				ReadNextToken();
				newOperatorStack.Push(_lookahead.StartPosition);
			}
			while (_lookahead.Type == TokenType.New);

			var result = ParseMemberExpression();

			// Обработать все MemberExpression
			while (_lookahead.Type == TokenType.LParenthesis && newOperatorStack.Count >= 0) {
				if (!result.CanBeConstructor)
					ThrowExpectedConstructor(newOperatorStack.Pop());
				var arguments = ParseArguments();
				result = EatMemberOperators(Expression.New(result, arguments));
				newOperatorStack.Pop();
			}

			// Теперь остались только операторы new относящиеся к NewExpression
			while (newOperatorStack.Count >= 0) {
				if (!result.CanBeConstructor)
					ThrowExpectedConstructor(newOperatorStack.Pop());
				result = Expression.New(result, EmptyArgumentList);
				newOperatorStack.Pop();
			}

			return (result);
		}

		private Expression ParsePostfixExpression() {
			var startPos = _lookahead.StartPosition;
			var result = ParseLeftHandSideExpression();
			if (_lookahead.Type == TokenType.Inc || _lookahead.Type == TokenType.Dec) {
				if (!result.IsReference)
					ThrowExpectedReference(startPos);
				result = _lookahead.Type == TokenType.Inc ? Expression.PostfixInc(result) : Expression.PostfixDec(result);
			}
			return (result);
		}

		private Expression ParseUnaryExpression() {
			Expression result;
			switch (_lookahead.Type) {
				case TokenType.Delete: {
						ReadNextToken();
						var startPos = _lookahead.StartPosition;
						var operand = ParsePostfixExpression();
						if (!operand.CanBeDeleted)
							ThrowExpectedReference(startPos);
						result = Expression.Delete(operand);
						break;
					}
				case TokenType.Void:
					ReadNextToken();
					result = Expression.Void(ParsePostfixExpression());
					break;
				case TokenType.Typeof:
					ReadNextToken();
					result = Expression.TypeOf(ParsePostfixExpression());
					break;
				case TokenType.Inc: {
						ReadNextToken();
						var startPos = _lookahead.StartPosition;
						var operand = ParsePostfixExpression();
						if (!operand.IsReference)
							ThrowExpectedReference(startPos);
						result = Expression.Inc(operand);
						break;
					}
				case TokenType.Dec: {
						ReadNextToken();
						var startPos = _lookahead.StartPosition;
						var operand = ParsePostfixExpression();
						if (!operand.IsReference)
							ThrowExpectedReference(startPos);
						result = Expression.Dec(operand);
						break;
					}
				case TokenType.Plus:
					ReadNextToken();
					result = Expression.Pos(ParsePostfixExpression());
					break;
				case TokenType.Minus:
					ReadNextToken();
					result = Expression.Neg(ParsePostfixExpression());
					break;
				case TokenType.BitNot:
					ReadNextToken();
					result = Expression.BitNot(ParsePostfixExpression());
					break;
				case TokenType.Not:
					ReadNextToken();
					result = Expression.Not(ParsePostfixExpression());
					break;
				default:
					result = ParsePostfixExpression();
					break;
			}
			return (result);
		}

		private Expression ParseMultiplicativeExpression() {
			var result = ParseUnaryExpression();
			while (true) {
				switch (_lookahead.Type) {
					case TokenType.Star:
						ReadNextToken();
						result = Expression.Mul(result, ParseUnaryExpression());
						break;
					case TokenType.Slash:
						ReadNextToken();
						result = Expression.Div(result, ParseUnaryExpression());
						break;
					case TokenType.Mod:
						ReadNextToken();
						result = Expression.Mod(result, ParseUnaryExpression());
						break;
					default:
						return (result);
				}
			}
		}

		private Expression ParseAdditiveExpression() {
			var result = ParseMultiplicativeExpression();
			while (true) {
				switch (_lookahead.Type) {
					case TokenType.Plus:
						ReadNextToken();
						result = Expression.Plus(result, ParseMultiplicativeExpression());
						break;
					case TokenType.Minus:
						ReadNextToken();
						result = Expression.Minus(result, ParseMultiplicativeExpression());
						break;
					default:
						return (result);
				}
			}
		}

		private Expression ParseShiftExpression() {
			var result = ParseAdditiveExpression();
			while (true) {
				switch (_lookahead.Type) {
					case TokenType.Shl:
						ReadNextToken();
						result = Expression.Shl(result, ParseAdditiveExpression());
						break;
					case TokenType.ShrS:
						ReadNextToken();
						result = Expression.ShrS(result, ParseAdditiveExpression());
						break;
					case TokenType.ShrU:
						ReadNextToken();
						result = Expression.ShrU(result, ParseAdditiveExpression());
						break;
					default:
						return (result);
				}
			}
		}

		private Expression ParseRelationalExpression() {
			var result = ParseShiftExpression();
			while (true) {
				switch (_lookahead.Type) {
					case TokenType.Lt:
						ReadNextToken();
						result = Expression.Lt(result, ParseShiftExpression());
						break;
					case TokenType.Lte:
						ReadNextToken();
						result = Expression.Lte(result, ParseShiftExpression());
						break;
					case TokenType.Gt:
						ReadNextToken();
						result = Expression.Lt(result, ParseShiftExpression());
						break;
					case TokenType.Gte:
						ReadNextToken();
						result = Expression.Lte(result, ParseShiftExpression());
						break;
					case TokenType.InstanceOf: {
							ReadNextToken();
							var startPos = _lookahead.StartPosition;
							var rightOperand = ParseShiftExpression();
							if (!rightOperand.CanBeConstructor)
								ThrowExpectedConstructor(startPos);
							result = Expression.InstanceOf(result, rightOperand);
							break;
						}
					case TokenType.In: {
							ReadNextToken();
							var startPos = _lookahead.StartPosition;
							var rightOperand = ParseShiftExpression();
							if (!rightOperand.CanHaveMembers)
								ThrowExpectedObject(startPos);
							result = Expression.In(result, rightOperand);
							break;
						}
					default:
						return (result);
				}
			}
		}

		private Expression ParseEqualityExpression() {
			var result = ParseRelationalExpression();
			while (true) {
				switch (_lookahead.Type) {
					case TokenType.Eq:
						ReadNextToken();
						result = Expression.Eq(result, ParseRelationalExpression());
						break;
					case TokenType.Neq:
						ReadNextToken();
						result = Expression.Neq(result, ParseRelationalExpression());
						break;
					case TokenType.StrictEq:
						ReadNextToken();
						result = Expression.StrictEq(result, ParseRelationalExpression());
						break;
					case TokenType.StrictNeq:
						ReadNextToken();
						result = Expression.StrictNeq(result, ParseRelationalExpression());
						break;
					default:
						return (result);
				}
			}
		}

		private Expression ParseBitwiseAndExpression() {
			var result = ParseEqualityExpression();
			while (_lookahead.Type == TokenType.BitAnd) {
				ReadNextToken();
				result = Expression.BitAnd(result, ParseEqualityExpression());
			}
			return (result);
		}

		private Expression ParseBitwiseXorExpression() {
			var result = ParseBitwiseAndExpression();
			while (_lookahead.Type == TokenType.BitXor) {
				ReadNextToken();
				result = Expression.BitXor(result, ParseBitwiseAndExpression());
			}
			return (result);
		}

		private Expression ParseBitwiseOrExpression() {
			var result = ParseBitwiseXorExpression();
			while (_lookahead.Type == TokenType.BitOr) {
				ReadNextToken();
				result = Expression.BitOr(result, ParseBitwiseXorExpression());
			}
			return (result);
		}

		private Expression ParseLogicalAndExpression() {
			var result = ParseBitwiseOrExpression();
			while (_lookahead.Type == TokenType.And) {
				ReadNextToken();
				result = Expression.And(result, ParseBitwiseOrExpression());
			}
			return (result);
		}

		private Expression ParseLogicalOrExpression() {
			var result = ParseLogicalAndExpression();
			while (_lookahead.Type == TokenType.Or) {
				ReadNextToken();
				result = Expression.Or(result, ParseLogicalAndExpression());
			}
			return (result);
		}

		private Expression ParseConditionalExpression() {
			var result = ParseLogicalOrExpression();
			if (_lookahead.Type != TokenType.QuestionMark)
				return (result);
			var trueOperand = ParseAssignmentExpression();
			Match(TokenType.Colon);
			var falseOperand = ParseAssignmentExpression();
			return (Expression.Conditional(result, trueOperand, falseOperand));
		}

		private Expression ParseAssignmentExpression() {
			var startPos = _lookahead.StartPosition;
			var result = ParseConditionalExpression();
			switch (_lookahead.Type) {
				case TokenType.Assign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.SimpleAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.PlusAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.PlusAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.MinusAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.MinusAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.StarAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.MulAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.SlashAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.DivAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.ModAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.ModAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.ShlAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.ShlAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.ShrSAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.ShrSAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.ShrUAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.ShrUAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.BitAndAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.BitAndAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.BitXorAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.BitXorAssign(result, ParseAssignmentExpression());
					break;
				case TokenType.BitOrAssign:
					if (!result.IsReference)
						ThrowExpectedReference(startPos);
					ReadNextToken();
					result = Expression.BitOrAssign(result, ParseAssignmentExpression());
					break;
			}
			return (result);
		}

		public Expression ParseExpression() {
			var assignment = ParseAssignmentExpression();
			if (_lookahead.Type != TokenType.Comma)
				return (assignment);
			var sequence = new List<Expression>() { assignment };
			do {
				ReadNextToken();
				sequence.Add(ParseAssignmentExpression());
			} while (_lookahead.Type == TokenType.Comma);
			return (Expression.Sequence(sequence));
		}
	}
}
