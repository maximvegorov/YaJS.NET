using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaJS.Compiler.AST;
using YaJS.Compiler.AST.Expressions;

namespace YaJS.Compiler.Tests {
	partial class ParserTests {
		private static Expression ParseExpression(string source) {
			var tokenizer = new Tokenizer(new StringReader(source));
			var parser = new Parser(tokenizer);
			return (parser.ParseExpression());
		}

		private static Dictionary<string, ExpressionType> GetPrimaryExpressions() {
			return (new Dictionary<string, ExpressionType>() {
				{ "arguments", ExpressionType.Arguments },
				{ "false", ExpressionType.BooleanLiteral },
				{ "true", ExpressionType.BooleanLiteral },
				{ "eval", ExpressionType.Eval },
				{ "1.0", ExpressionType.FloatLiteral },
				{ "a", ExpressionType.Ident },
				{ "10", ExpressionType.IntegerLiteral },
				{ "0x10", ExpressionType.IntegerLiteral },
				{ "null", ExpressionType.NullLiteral },
				{ "\"abc\"", ExpressionType.StringLiteral },
				{ "'abc'", ExpressionType.StringLiteral },
				{ "this", ExpressionType.This },
				{ "undefined", ExpressionType.UndefinedLiteral },
			});
		}

		[TestMethod]
		public void PrimaryExpressions() {
			foreach (var expression in GetPrimaryExpressions())
				Assert.IsTrue(ParseExpression(expression.Key).Type == expression.Value, expression.Key);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleArrayLiteral() {
			var result = ParseExpression("['a', 'b', 1]") as ArrayLiteral;
			Assert.IsTrue(result != null && result.Items.Count == 3);
			Assert.IsTrue(result.Items[0].Type == ExpressionType.StringLiteral);
			Assert.IsTrue(result.Items[1].Type == ExpressionType.StringLiteral);
			Assert.IsTrue(result.Items[2].Type == ExpressionType.IntegerLiteral);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleArrayLiteral_MissingElements() {
			var result = ParseExpression("[,,'a',,, 'b',,]") as ArrayLiteral;
			Assert.IsTrue(result != null && result.Items.Count == 7);
			Assert.IsTrue(result.Items[0].Type == ExpressionType.UndefinedLiteral);
			Assert.IsTrue(result.Items[1].Type == ExpressionType.UndefinedLiteral);
			Assert.IsTrue(result.Items[2].Type == ExpressionType.StringLiteral);
			Assert.IsTrue(result.Items[3].Type == ExpressionType.UndefinedLiteral);
			Assert.IsTrue(result.Items[4].Type == ExpressionType.UndefinedLiteral);
			Assert.IsTrue(result.Items[5].Type == ExpressionType.StringLiteral);
			Assert.IsTrue(result.Items[6].Type == ExpressionType.UndefinedLiteral);
		}

		[TestMethod]
		public void PrimaryExpressions_SimpleObjectLiteral() {
			var result = ParseExpression("{a: true, b: 1, c: 'a', 1: null}") as ObjectLiteral;
			Assert.IsTrue(result != null && result.Properties.Count == 4);
			Assert.IsTrue(result.Properties[0].Key == "a" && result.Properties[0].Value.Type == ExpressionType.BooleanLiteral);
			Assert.IsTrue(result.Properties[1].Key == "b" && result.Properties[1].Value.Type == ExpressionType.IntegerLiteral);
			Assert.IsTrue(result.Properties[2].Key == "c" && result.Properties[2].Value.Type == ExpressionType.StringLiteral);
			Assert.IsTrue(result.Properties[3].Key == "1" && result.Properties[3].Value.Type == ExpressionType.NullLiteral);
		}
	}
}
