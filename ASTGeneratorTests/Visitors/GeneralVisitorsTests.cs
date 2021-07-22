using Antlr4.Runtime;
using Exceptions.Exceptions.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTGenerator.Visitors.Tests {
	[TestClass()]
	public class GeneralVisitorsTests {

		#region VisitGroupingExpr Tests
		[TestMethod()]
		[DataRow("( 1 )", typeof(IntLiteralNode))]
		[DataRow("( a )", typeof(IdentifierExpr))]
		[DataRow("( a + b )", typeof(BinaryExprNode))]
		public void VisitGroupingExpr_ContextTest(string input, Type valueType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			GroupingExprNode node = (GroupingExprNode)aSTVisitors.VisitGroupingExpr((JanCParser.GroupingExprContext)cst);

			// ASSERT
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("( 1 )", typeof(IntLiteralNode))]
		[DataRow("( a )", typeof(IdentifierExpr))]
		[DataRow("( a + b )", typeof(BinaryExprNode))]
		public void VisitGroupingExpr_TypeTest(string input, Type valueType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			GroupingExprNode node = (GroupingExprNode)aSTVisitors.VisitGroupingExpr((JanCParser.GroupingExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, valueType);
		}

		#endregion

		#region VisitAssignStmt Tests
		[TestMethod()]
		[DataRow("a = 1")]
		[DataRow("a = b")]
		[DataRow("a += 1")]
		[DataRow("a -= 1")]
		[DataRow("a /= 1")]
		[DataRow("a *= 1")]
		public void VisitAssignStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt((JanCParser.AssignStmtContext)cst);

			// ASSERT
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("a = 1", AssignOperator.Assign)]
		[DataRow("a = b", AssignOperator.Assign)]
		[DataRow("a += 1", AssignOperator.PlusAssign)]
		[DataRow("a -= 1", AssignOperator.MinusAssign)]
		[DataRow("a /= 1", AssignOperator.DivideAssign)]
		[DataRow("a *= 1", AssignOperator.MultiplyAssign)]
		public void VisitAssignStmt_OperatorTest(string input, AssignOperator expectedOperator) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt((JanCParser.AssignStmtContext)cst);

			// ASSERT
			Assert.AreEqual(expectedOperator, node.Operator);
		}
		[TestMethod()]
		[DataRow("a = 1", typeof(IdentifierExpr), typeof(IntLiteralNode))]
		[DataRow("a = b", typeof(IdentifierExpr), typeof(IdentifierExpr))]
		[DataRow("a += 1", typeof(IdentifierExpr), typeof(IntLiteralNode))]
		[DataRow("a -= 1", typeof(IdentifierExpr), typeof(IntLiteralNode))]
		[DataRow("a /= 1", typeof(IdentifierExpr), typeof(IntLiteralNode))]
		[DataRow("a *= 1", typeof(IdentifierExpr), typeof(IntLiteralNode))]
		public void VisitAssignStmt_TypeTest(string input, Type locationType, Type valueType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt((JanCParser.AssignStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, valueType);
			Assert.IsInstanceOfType(node.Location, locationType);
		}
		[TestMethod()]
		[DataRow("a = 1", "a")]
		[DataRow("ab = b", "ab")]
		[DataRow("a += 1", "a")]
		[DataRow("baba -= 1", "baba")]
		[DataRow("a /= 1", "a")]
		[DataRow("a *= 1", "a")]
		public void VisitAssignStmt_IdentifierTest(string input, string expectedName) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt((JanCParser.AssignStmtContext)cst);

			// ASSERT
			Assert.AreEqual(expectedName, ((IdentifierExpr)node.Location).Name);
		}
		[TestMethod()]
		[DataRow("a = 1", 1)]
		[DataRow("a += 100", 100)]
		[DataRow("a -= 1012345130", 1012345130)]
		public void VisitAssignStmt_IntValueTest(string input, int expectedValue) {
			// ARRANGE
			var lexer = new JanCLexer(new AntlrInputStream(input));
			var parser = new JanCParser(new CommonTokenStream(lexer));

			var cst = (ParserRuleContext)parser.stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt((JanCParser.AssignStmtContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((IntLiteralNode)node.Value).Value);
		}
		[TestMethod()]
		[DataRow("a = \"1\"", "1")]
		[DataRow("a = \"1abba1\"", "1abba1")]
		[DataRow("a = \"-1\"", "-1")]
		public void VisitAssignStmt_StringValueTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt((JanCParser.AssignStmtContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((StringLiteralNode)node.Value).Value);
		}
		[TestMethod()]
		[ExpectedException(typeof(UnreachableException))]
		public void VisitAssignStmt_BadOperatorTest() {
			// ARRANGE
			var cst = (ParserRuleContext)getParser("a += 1").stmt();
			ITokenFactory tokenFactory = new CommonTokenFactory();
			JanCParser.AssignStmtContext context = (JanCParser.AssignStmtContext)cst;
			context.op = tokenFactory.Create(0, "^");
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			AssignStmtNode node = (AssignStmtNode)aSTVisitors.VisitAssignStmt(context);

			// ASSERT
		}
		#endregion

		#region VisitIdentifierExpr Tests
		[TestMethod()]
		[DataRow("a")]
		[DataRow("a_b")]
		[DataRow("sdfajfa34asda")]
		public void VisitIdentifierExpr_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IdentifierExpr node = (IdentifierExpr)aSTVisitors.VisitIdentifierExpr((JanCParser.IdentifierExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.IdentifierContext));
			Assert.AreEqual(((JanCParser.IdentifierExprContext)cst).identifier(), node.Context);
		}
		[TestMethod()]
		[DataRow("a", "a")]
		[DataRow("a_b", "a_b")]
		[DataRow("sdfajfa34asda", "sdfajfa34asda")]
		public void VisitIdentifierExpr_NameTest(string input, string expectedName) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IdentifierExpr node = (IdentifierExpr)aSTVisitors.VisitIdentifierExpr((JanCParser.IdentifierExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.IdentifierContext));
			Assert.AreEqual(expectedName, node.Name);
		}
		#endregion

		#region VisitCallExpr Tests
		[TestMethod()]
		[DataRow("a()")]
		[DataRow("a(1)")]
		[DataRow("a(1, \"a\")")]
		public void VisitCallExpr_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.CallExprContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("a()")]
		[DataRow("a(          )")]
		public void VisitCallExpr_NoArgumentsTypeTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			Assert.AreEqual(0, node.Arguments.Count);
		}
		[TestMethod()]
		[DataRow("a(1)", typeof(IntLiteralNode))]
		[DataRow("a(1, \"a\")", typeof(IntLiteralNode))]
		public void VisitCallExpr_ArgumentTypeTest(string input, Type expectedArgumentType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Arguments[0].Value, expectedArgumentType);
		}
		[TestMethod()]
		[DataRow("a()", null, null)]
		[DataRow("a(1)", typeof(IntLiteralNode), null)]
		[DataRow("a(1, \"a\")", typeof(IntLiteralNode), typeof(StringLiteralNode))]
		public void VisitCallExpr_ArgumentsTypeTest(string input, Type firstArgumentType, Type secondArgumentType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			if (firstArgumentType != null)
				Assert.IsInstanceOfType(node.Arguments[0].Value, firstArgumentType);
			if (secondArgumentType != null)
				Assert.IsInstanceOfType(node.Arguments[1].Value, secondArgumentType);
		}
		[TestMethod()]
		[DataRow("a(\"a\")", "a")]
		[DataRow("a(\"abc\")", "abc")]
		[DataRow("a(\"a123b\")", "a123b")]
		public void VisitCallExpr_ArgumentValueStringTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((StringLiteralNode)node.Arguments[0].Value).Value);
		}
		[TestMethod()]
		[DataRow("a(1)", 1)]
		[DataRow("a(152)", 152)]
		[DataRow("a(00000001)", 1)]
		public void VisitCallExpr_ArgumentValueIntTest(string input, int expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((IntLiteralNode)node.Arguments[0].Value).Value);
		}
		[TestMethod()]
		[DataRow("a(1.5)", 1.5)]
		[DataRow("a(152.12e4)", 152.12e4)]
		[DataRow("a(00000001.5)", 1.5)]
		public void VisitCallExpr_ArgumentValueFloatTest(string input, double expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CallExprNode node = (CallExprNode)aSTVisitors.VisitCallExpr((JanCParser.CallExprContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((FloatLiteralNode)node.Arguments[0].Value).Value);
		}
		#endregion

		#region VisitArgument Tests
		[TestMethod()]
		[DataRow("", null)]
		[DataRow("1", typeof(IntLiteralNode))]
		[DataRow("\"a\"", typeof(StringLiteralNode))]
		public void VisitArgument_ContextTest(string input, Type argumentType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).argument();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ArgNode node = (ArgNode)aSTVisitors.VisitArgument((JanCParser.ArgumentContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.ArgumentContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("", null)]
		[DataRow("1", typeof(IntLiteralNode))]
		[DataRow("\"a\"", typeof(StringLiteralNode))]
		public void VisitArgument_TypeTest(string input, Type argumentType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).argument();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ArgNode node = (ArgNode)aSTVisitors.VisitArgument((JanCParser.ArgumentContext)cst);

			// ASSERT
			if (argumentType != null)
				Assert.IsInstanceOfType(node.Value, argumentType);
			else
				Assert.AreEqual(node.Value, argumentType);
		}
		[TestMethod()]
		[DataRow("\"a\"", "a")]
		[DataRow("\"aa123\"", "aa123")]
		[DataRow("\"1234\"", "1234")]
		public void VisitArgument_StringValueTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).argument();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ArgNode node = (ArgNode)aSTVisitors.VisitArgument((JanCParser.ArgumentContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((StringLiteralNode)node.Value).Value);
		}
		#endregion

		#region VisitNumberLiteral Tests
		[TestMethod()]
		[DataRow("1", typeof(IntLiteralNode))]
		[DataRow("1.1", typeof(FloatLiteralNode))]
		public void VisitNumberLiteral_TypeTest(string input, Type expectedNodeType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ILiteral node = (ILiteral)aSTVisitors.VisitNumberLiteral((JanCParser.NumberLiteralContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node, expectedNodeType);
		}
		[TestMethod()]
		[DataRow("1", 1)]
		[DataRow("100", 100)]
		[DataRow("1352352342", 1352352342)]
		public void VisitNumberLiteral_IntTest(string input, int expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IntLiteralNode node = (IntLiteralNode)aSTVisitors.VisitNumberLiteral((JanCParser.NumberLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, node.Value);
		}
		[TestMethod()]
		[DataRow("1.1", 1.1)]
		[DataRow("100.1e-1", 100.1e-1)]
		[DataRow("1e5", 1e5)]
		public void VisitNumberLiteral_FloatTest(string input, double expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FloatLiteralNode node = (FloatLiteralNode)aSTVisitors.VisitNumberLiteral((JanCParser.NumberLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, node.Value);
		}
		[TestMethod()]
		[ExpectedException(typeof(UnreachableException))]
		public void VisitNumberLiteral_ExeptionTest() {
			// ARRANGE
			JanCParser.NumberLiteralContext failContext = new JanCParser.NumberLiteralContext(new JanCParser.ExprContext());
			ITokenFactory tokenFactory = new CommonTokenFactory();
			failContext.value = tokenFactory.Create(0, "ab1");
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ILiteral node = (ILiteral)aSTVisitors.VisitNumberLiteral(failContext);

			// ASSERT
		}
		#endregion

		#region VisitStringLiteral Tests
		[TestMethod()]
		[DataRow("\"1\"")]
		[DataRow("\"1baba\"")]
		public void VisitStringLiteral_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StringLiteralNode node = (StringLiteralNode)aSTVisitors.VisitStringLiteral((JanCParser.StringLiteralContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.StringLiteralContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("\"1\"", "1")]
		[DataRow("\"1baba\"", "1baba")]
		public void VisitStringLiteral_ValueTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StringLiteralNode node = (StringLiteralNode)aSTVisitors.VisitStringLiteral((JanCParser.StringLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, node.Value);
		}
		#endregion

		#region VisitStructLiteral Tests
		[TestMethod()]
		[DataRow("abc{}")]
		[DataRow("a{}")]
		public void VisitStructLiteral_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.StructLiteralContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("abc{}", "abc")]
		[DataRow("a{}", "a")]
		public void VisitStructLiteral_TypeTest(string input, string expectedTypeName) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedTypeName, node.Type.Name);
		}
		[TestMethod()]
		[DataRow("abc{}")]
		[DataRow("abc{            }")]
		public void VisitStructLiteral_NoMembersTypeTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(0, node.Members.Count);
		}
		[TestMethod()]
		[DataRow("abc{1}", typeof(IntLiteralNode))]
		[DataRow("abc{1, \"1\"}", typeof(IntLiteralNode))]
		public void VisitStructLiteral_MemberTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Members[0].Value, expectedType);
		}
		[TestMethod()]
		[DataRow("abc{}", null, null)]
		[DataRow("abc{1}", typeof(IntLiteralNode), null)]
		[DataRow("abc{1, \"1\"}", typeof(IntLiteralNode), typeof(StringLiteralNode))]
		public void VisitStructLiteral_MembersTypeTest(string input, Type firstMemberType, Type secondMemberType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			if (firstMemberType != null)
				Assert.IsInstanceOfType(node.Members[0].Value, firstMemberType);
			if (secondMemberType != null)
				Assert.IsInstanceOfType(node.Members[1].Value, secondMemberType);
		}
		[TestMethod()]
		[DataRow("abc{1}", 1)]
		[DataRow("abc{1000}", 1000)]
		public void VisitStructLiteral_MemberValueIntTest(string input, int expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((IntLiteralNode)node.Members[0].Value).Value);
		}
		[TestMethod()]
		[DataRow("abc{\"1\"}", "1")]
		[DataRow("abc{\"1000\"}", "1000")]
		[DataRow("abc{\"1aba000\"}", "1aba000")]
		public void VisitStructLiteral_MemberValueStringTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralNode node = (StructLiteralNode)aSTVisitors.VisitStructLiteral((JanCParser.StructLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((StringLiteralNode)node.Members[0].Value).Value);
		}

		#endregion

		#region VisitStructLiteralMember Tests
		[TestMethod()]
		[DataRow("int a")]
		[DataRow("str b")]
		public void VisitStructLiteralMember_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).structLiteralMember();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralMemberNode node = (StructLiteralMemberNode)aSTVisitors.VisitStructLiteralMember((JanCParser.StructLiteralMemberContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.StructLiteralMemberContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("a", typeof(IdentifierExpr))]
		[DataRow("b", typeof(IdentifierExpr))]
		[DataRow("5", typeof(IntLiteralNode))]
		[DataRow("a = 56", typeof(IntLiteralNode))]
		public void VisitStructLiteralMember_TypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).structLiteralMember();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructLiteralMemberNode node = (StructLiteralMemberNode)aSTVisitors.VisitStructLiteralMember((JanCParser.StructLiteralMemberContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, expectedType);
		}

		#endregion

		#region VisitMemberAccess Tests
		[TestMethod()]
		[DataRow("structname.a")]
		[DataRow("structname.more")]
		public void VisitMemberAccess_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			MemberAccessNode node = (MemberAccessNode)aSTVisitors.VisitMemberAccess((JanCParser.MemberAccessContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.MemberAccessContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("structname.a", "a")]
		[DataRow("structname.more", "more")]
		public void VisitMemberAccess_MemberNameTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			MemberAccessNode node = (MemberAccessNode)aSTVisitors.VisitMemberAccess((JanCParser.MemberAccessContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, node.MemberName);
		}
		[TestMethod()]
		[DataRow("structname.a", "structname")]
		[DataRow("structname.more", "structname")]
		public void VisitMemberAccess_StructNameTest(string input, string expectedValue) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			MemberAccessNode node = (MemberAccessNode)aSTVisitors.VisitMemberAccess((JanCParser.MemberAccessContext)cst);

			// ASSERT
			Assert.AreEqual(expectedValue, ((IdentifierExpr)node.Item).Name);
		}
		[TestMethod()]
		[DataRow("structname.otherstructname.a", "a")]
		[DataRow("structname.otherstructname.more", "more")]
		public void VisitMemberAccess_MemberInMemeberTest(string input, string identifierName) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			MemberAccessNode node = (MemberAccessNode)aSTVisitors.VisitMemberAccess((JanCParser.MemberAccessContext)cst);

			// ASSERT
			Assert.AreEqual(identifierName, node.MemberName);
			Assert.AreEqual("otherstructname", ((MemberAccessNode)node.Item).MemberName);
			Assert.AreEqual("structname", ((IdentifierExpr)((MemberAccessNode)node.Item).Item).Name);
		}
		#endregion

		#region VisitReturnStmt Tests
		[TestMethod()]
		[DataRow("return")]
		[DataRow("return 0")]
		[DataRow("return func()")]
		public void VisitReturnStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ReturnStmtNode node = (ReturnStmtNode)aSTVisitors.VisitReturnStmt((JanCParser.ReturnStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.ReturnStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("return", null)]
		[DataRow("return 0", typeof(IntLiteralNode))]
		[DataRow("return func()", typeof(CallExprNode))]
		public void VisitReturnStmt_TypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ReturnStmtNode node = (ReturnStmtNode)aSTVisitors.VisitReturnStmt((JanCParser.ReturnStmtContext)cst);

			// ASSERT
			if (expectedType != null)
				Assert.IsInstanceOfType(node.Value, expectedType);
			else
				Assert.AreEqual(expectedType, node.Value);
		}
		#endregion

		#region Private Test Methods

		private static JanCParser getParser(string input) {
			var lexer = new JanCLexer(new AntlrInputStream(input));
			return new JanCParser(new CommonTokenStream(lexer));
		}

		#endregion
	}
}
