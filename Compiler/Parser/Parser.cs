using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LementPro.Core.Expressions {
	using LementPro.Core.Expressions.Nodes;

	/// <summary>
	/// Синтаксис выражений в BNF:
	/// Expression ::= SimpleExpression ((= | <> | < | <= | >=) SimpleExpression)*
	/// SimpleExpression ::= Term ((+ | - | or | xor) Term)*
	/// Term ::= Factor ((* | / | % | and) Factor)*
	/// Factor ::= null | true | false | Number | String |
	///		-Factor | +Factor | not Factor |
	///		(Expression) |
	///		iff(Expression, Expression, Expression)
	///		VariableName | FunctionName([Expression(, Expression)*])
	/// </summary>
	internal sealed class Parser {
		private Tokenizer tokenizer;
		private ISet<string> variables;

		public Parser(string source) {
			tokenizer = new Tokenizer(source);
			variables = new HashSet<string>();
			tokenizer.ReadToken();
		}

		private void Match(TokenType expectedType) {
			if (tokenizer.CurToken.Type != expectedType)
				throw new ParserException(string.Format("Expected {0}", expectedType.ToString()));
			tokenizer.ReadToken();
		}

		private ExpressionTreeNode ParseIff() {
			Match(TokenType.LParenthesis);
			var condExpr = ParseExpression();
			Match(TokenType.Comma);
			var trueExpr = ParseExpression();
			Match(TokenType.Comma);
			var falseExpr = ParseExpression();
			Match(TokenType.RParenthesis);
			return (new IffOperator(condExpr, trueExpr, falseExpr));
		}

		private IEnumerable<ExpressionTreeNode> ParseParameterList() {
			yield return ParseExpression();
			while (tokenizer.CurToken.Type == TokenType.Comma) {
				tokenizer.ReadToken();
				yield return ParseExpression();
			}
			Match(TokenType.RParenthesis);
		}

		ExpressionTreeNode ParseFactor() {
			var tokenType = tokenizer.CurToken.Type;
			switch (tokenType) {
				case TokenType.Null:
					tokenizer.ReadToken();
					return (new ConstantOperand(ValueObject.Null));
				case TokenType.True:
				case TokenType.False:
					tokenizer.ReadToken();
					return (new ConstantOperand(tokenType == TokenType.True));
				case TokenType.Number:
					double numberValue;
					if (!double.TryParse(tokenizer.CurToken.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out numberValue))
						numberValue = double.NaN;
					tokenizer.ReadToken();
					return (new ConstantOperand(numberValue));
				case TokenType.String:
					var stringValue = tokenizer.CurToken.Value;
					tokenizer.ReadToken();
					return (new ConstantOperand(stringValue));
				case TokenType.Minus:
					tokenizer.ReadToken();
					return (new NegOperator(ParseFactor()));
				case TokenType.Plus:
					tokenizer.ReadToken();
					return (ParseFactor());
				case TokenType.Not:
					tokenizer.ReadToken();
					return (new NotOperator(ParseFactor()));
				case TokenType.LParenthesis:
					tokenizer.ReadToken();
					var expr = ParseExpression();
					Match(TokenType.RParenthesis);
					return (expr);
				case TokenType.Iff:
					tokenizer.ReadToken();
					return (ParseIff());
				case TokenType.Ident:
					var ident = tokenizer.CurToken.Value;
					tokenizer.ReadToken();
					if (tokenizer.CurToken.Type != TokenType.LParenthesis) {
						variables.Add(ident);
						return (new VariableOperand(ident));
					}
					tokenizer.ReadToken();
					if (tokenizer.CurToken.Type == TokenType.RParenthesis) {
						tokenizer.ReadToken();
						return (new CallFuncOperator(ident, null));
					}
					return (new CallFuncOperator(ident, ParseParameterList().ToList()));
				default:
					throw new ParserException(
						string.Format("Unexpected token {0}", tokenType)
					);
			}
		}

		private ExpressionTreeNode ParseTerm() {
			var result = ParseFactor();
			var canParse = true;
			while (canParse) {
				var tokenType = tokenizer.CurToken.Type;
				switch (tokenType) {
					case TokenType.Star:
						tokenizer.ReadToken();
						result = new MulOperator(result, ParseFactor());
						break;
					case TokenType.Slash:
						tokenizer.ReadToken();
						result = new DivOperator(result, ParseFactor());
						break;
					case TokenType.Percent:
						tokenizer.ReadToken();
						result = new ModOperator(result, ParseFactor());
						break;
					case TokenType.And:
						tokenizer.ReadToken();
						result = new AndOperator(result, ParseFactor());
						break;
					default:
						canParse = false;
						break;
				}
			}
			return (result);
		}

		private ExpressionTreeNode ParseSimpleExpression() {
			var result = ParseTerm();
			var canParse = true;
			while (canParse) {
				var tokenType = tokenizer.CurToken.Type;
				switch (tokenType) {
					case TokenType.Plus:
						tokenizer.ReadToken();
						result = new PlusOperator(result, ParseTerm());
						break;
					case TokenType.Minus:
						tokenizer.ReadToken();
						result = new MinusOperator(result, ParseTerm());
						break;
					case TokenType.Or:
						tokenizer.ReadToken();
						result = new OrOperator(result, ParseTerm());
						break;
					case TokenType.Xor:
						tokenizer.ReadToken();
						result = new XorOperator(result, ParseTerm());
						break;
					default:
						canParse = false;
						break;
				}
			}
			return (result);
		}

		private ExpressionTreeNode ParseExpression() {
			var result = ParseSimpleExpression();
			var canParse = true;
			while (canParse) {
				var tokenType = tokenizer.CurToken.Type;
				switch (tokenType) {
					case TokenType.Equal:
						tokenizer.ReadToken();
						result = new EqualOperator(result, ParseSimpleExpression());
						break;
					case TokenType.NotEqual:
						tokenizer.ReadToken();
						result = new NotEqualOperator(result, ParseSimpleExpression());
						break;
					case TokenType.Lt:
						tokenizer.ReadToken();
						result = new LtOperator(result, ParseSimpleExpression());
						break;
					case TokenType.Lte:
						tokenizer.ReadToken();
						result = new XorOperator(result, ParseSimpleExpression());
						break;
					case TokenType.Gt:
						tokenizer.ReadToken();
						result = new LtOperator(result, ParseSimpleExpression());
						break;
					case TokenType.Gte:
						tokenizer.ReadToken();
						result = new XorOperator(result, ParseSimpleExpression());
						break;
					default:
						canParse = false;
						break;
				}
			}
			return (result);
		}

		public ExpressionTreeNode Parse(out List<string> variableList) {
			var result = ParseExpression();
			if (tokenizer.CurToken.Type != TokenType.Unknown)
				throw new ParserException("Unexpected end of expression.");
			variableList = variables.ToList();
			return (result);
		}
	}
}
