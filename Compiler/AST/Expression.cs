using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaJS.Compiler.AST {
	/// <summary>
	/// Синтаксис выражений:
	/// Arguments ::= [Expression(, Expression)*] 
	/// PrimaryExpression ::= undefined | null | false | true | Number | String |
	///		Identifier | ObjectLiteral | ArrayLiteral | FunctionLiteral | this |
	///		(Expression)
	/// MemberExpression ::= PrimaryExpression ([Expression] | .Identifier)* |
	///		new MemberExpression(Arguments) ([Expression] | .Identifier)*
	/// LeftHandSideExpression ::= new+ MemberExpression | MemberExpression [(Arguments) ([Expression] | .Identifier)*]
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
	/// BitwiseOrExpression ::= BitwiseXorExpression (^ BitwiseXorExpression)*
	/// BitwiseOrExpressionNoIn ::= BitwiseXorExpressionNoIn (^ BitwiseXorExpressionNoIn)*
	/// LogicalAndExpression ::= BitwiseOrExpression (& BitwiseOrExpression)*
	/// LogicalAndExpressionNoIn ::= BitwiseOrExpressionNoIn (& BitwiseOrExpressionNoIn)*
	/// LogicalOrExpression ::= LogicalAndExpression (^ LogicalAndExpression)*
	/// LogicalOrExpressionNoIn ::= LogicalAndExpressionNoIn (^ LogicalAndExpressionNoIn)*
	/// ConditionalExpression ::= LogicalOrExpression [? AssignmentExpression : AssignmentExpression]
	/// ConditionalExpressionNoIn ::= LogicalOrExpressionNoIn [? AssignmentExpression : AssignmentExpressionNoIn]
	/// AssignmentExpression ::= ConditionalExpression |
	///		LeftHandSideExpression ((= | */ | /= | %= | += | -= | &lt;&lt;= | &gt;&gt;= | &gt;&gt;&gt;= | &= | ^= | |=) ConditionalExpression)*
	/// AssignmentExpressionNoIn ::= ConditionalExpressionNoIn |
	///		LeftHandSideExpression ((= | */ | /= | %= | += | -= | &lt;&lt;= | &gt;&gt;= | &gt;&gt;&gt;= | &= | ^= | |=) ConditionalExpressionNoIn)*
	/// Expression ::= AssignmentExpression (, AssignmentExpression)*
	/// ExpressionNoIn ::= AssignmentExpressionNoIn (, AssignmentExpressionNoIn)*
	/// </summary>
	public abstract class Expression {
	}
}
