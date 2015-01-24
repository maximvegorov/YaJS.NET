using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Expressions;
using YaJS.Compiler.AST.Statements;
using YaJS.Compiler.Exceptions;

namespace YaJS.Compiler.Tests {
	partial class ParserTests {
		private static Dictionary<string, ExpressionType> GetPrimaryExpressions() {
			return (new Dictionary<string, ExpressionType> {
				{"arguments", ExpressionType.Arguments},
				{"false", ExpressionType.BooleanLiteral},
				{"true", ExpressionType.BooleanLiteral},
				{"eval", ExpressionType.Eval},
				{"1.0", ExpressionType.FloatLiteral},
				{"a", ExpressionType.Ident},
				{"10", ExpressionType.IntegerLiteral},
				{"0x10", ExpressionType.IntegerLiteral},
				{"null", ExpressionType.NullLiteral},
				{"\"abc\"", ExpressionType.StringLiteral},
				{"'abc'", ExpressionType.StringLiteral},
				{"this", ExpressionType.This},
				{"undefined", ExpressionType.UndefinedLiteral}
			});
		}

		[TestMethod]
		public void PrimaryExpressions() {
			foreach (var expression in GetPrimaryExpressions())
				Assert.IsTrue(ParseExpression(expression.Key).Type == expression.Value, expression.Key);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleArrayLiteral() {
			var expected = Expression.Array(new List<Expression> {
				Expression.Undefined(), Expression.Null(), Expression.False(), Expression.True(), Expression.Integer("1"),
				Expression.String("a")
			}).ToString();
			var actual = ParseExpression(expected) as ArrayLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.ToString(), expected);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleArrayLiteral_MissingElements() {
			var expected = Expression.Array(new List<Expression> {
				Expression.Undefined(), Expression.Undefined(), Expression.String("a"), Expression.Undefined(),
				Expression.String("b"), Expression.Undefined()
			}).ToString();
			var actual = ParseExpression("[,,'a',,'b',,]") as ArrayLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.ToString(), expected);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleObjectLiteral() {
			var expected = Expression.Object(new List<KeyValuePair<string, Expression>> {
				new KeyValuePair<string, Expression>("1", Expression.Undefined()),
				new KeyValuePair<string, Expression>("2", Expression.Null()),
				new KeyValuePair<string, Expression>("a", Expression.True()),
				new KeyValuePair<string, Expression>("b", Expression.Integer("1")),
				new KeyValuePair<string, Expression>("c", Expression.String("a"))
			}).ToString();
			var actual = ParseExpression("{1: undefined, 2: null, a: true, b: 1, c: 'a'}") as ObjectLiteral;
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.ToString(), expected);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleGrouping() {
			foreach (var expression in GetPrimaryExpressions()) {
				var actual = ParseExpression("(" + expression.Key + ")") as GroupingOperator;
				Assert.IsNotNull(actual);
				Assert.IsTrue(actual.Operand.Type == expression.Value, expression.Key);
			}
		}

		[TestMethod]
		public void PrimaryExpressions_ComplexGrouping() {
			var expected = Expression.Grouping(
				Expression.Plus(
					Expression.Member(Expression.Ident("a"), Expression.Integer("0")),
					Expression.Mul(Expression.Ident("b"), Expression.Ident("c"))
					)
				).ToString();
			var actual = ParseExpression(expected) as GroupingOperator;
			Assert.IsNotNull(actual);
			Assert.AreEqual(actual.ToString(), expected);
		}

		[TestMethod]
		public void PrimaryExpressions_FunctionLiteral() {
			var expected = "(" +
				new Function(
					null,
					1,
					new List<string>() {"a", "b", "c"},
					new List<string>(),
					new List<Function>(),
					new FunctionBodyStatement() {
						new ReturnStatement(1, Expression.Integer(0))
					},
					false
					).ToString() +
				")";
			var actual = ParseFunction(expected);
			Assert.AreEqual(actual.NestedFunctions.Count, 1);
			Assert.AreEqual(actual.NestedFunctions.Count, 1);
			Assert.AreEqual(actual.FunctionBody.Statements.Count, 1);
			Assert.AreEqual(actual.FunctionBody.Statements[0].Type, StatementType.Expression);
			var actualStatement = actual.FunctionBody.Statements[0] as ExpressionStatement;
			Assert.IsNotNull(actualStatement);
			Assert.AreEqual(actualStatement.Expression.ToString(), expected);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidTokenException))]
		public void PrimaryExpressions_InvalidToken() {
			ParseExpression("+");
		}
	}
}
