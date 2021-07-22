using Antlr4.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTGenerator.Visitors.Tests {
	[TestClass()]
	public class ConditionalVisitorsTests {
		#region VisitIfStmt Tests
		[TestMethod()]
		[DataRow("if(1) {}")]
		[DataRow("if(1) {} else {}")]
		[DataRow("if(1) a")]
		public void VisitIfStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IfStmtNode node = (IfStmtNode)aSTVisitors.VisitIfStmt((JanCParser.IfStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.IfStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("if(1) {}", typeof(IntLiteralNode))]
		[DataRow("if(a) {} else {}", typeof(IdentifierExpr))]
		[DataRow("if(a == b) a", typeof(BinaryExprNode))]
		public void VisitIfStmt_ConditionTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IfStmtNode node = (IfStmtNode)aSTVisitors.VisitIfStmt((JanCParser.IfStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Condition, expectedType);
		}
		[TestMethod()]
		[DataRow("if(1) {}", typeof(BlockNode))]
		[DataRow("if(1) {} else {}", typeof(BlockNode))]
		[DataRow("if(1) a", typeof(IdentifierExpr))]
		[DataRow("if(1) a = 5", typeof(AssignStmtNode))]
		public void VisitIfStmt_ThenTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IfStmtNode node = (IfStmtNode)aSTVisitors.VisitIfStmt((JanCParser.IfStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}
		[TestMethod()]
		[DataRow("if(1) {}", null)]
		[DataRow("if(1) {} else {}", typeof(BlockNode))]
		[DataRow("if(1) {} else a", typeof(IdentifierExpr))]
		[DataRow("if(1) a",null)]
		[DataRow("if(1) a else {}", typeof(BlockNode))]
		[DataRow("if(1) a else b", typeof(IdentifierExpr))]
		public void VisitIfStmt_ElseTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IfStmtNode node = (IfStmtNode)aSTVisitors.VisitIfStmt((JanCParser.IfStmtContext)cst);

			// ASSERT
			if (expectedType != null)
				Assert.IsInstanceOfType(node.ElseBody, expectedType);
			else
				Assert.AreEqual(expectedType, node.ElseBody);
		}
		#endregion

		#region VisitWhileStmt Tests
		[TestMethod()]
		[DataRow("while(1) {}")]
		[DataRow("while(a) {}")]
		[DataRow("while(1) a")]
		public void VisitWhileStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			WhileStmtNode node = (WhileStmtNode)aSTVisitors.VisitWhileStmt((JanCParser.WhileStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.WhileStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("while(1) {}", typeof(IntLiteralNode))]
		[DataRow("while(a) {}", typeof(IdentifierExpr))]
		[DataRow("while(a < 5) a", typeof(BinaryExprNode))]
		public void VisitWhileStmt_ConditionTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			WhileStmtNode node = (WhileStmtNode)aSTVisitors.VisitWhileStmt((JanCParser.WhileStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Condition, expectedType);
		}
		[TestMethod()]
		[DataRow("while(1) {}", typeof(BlockNode))]
		[DataRow("while(a) {}", typeof(BlockNode))]
		[DataRow("while(1) a", typeof(IdentifierExpr))]
		[DataRow("while(1) a += 1", typeof(AssignStmtNode))]
		public void VisitWhileStmt_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			WhileStmtNode node = (WhileStmtNode)aSTVisitors.VisitWhileStmt((JanCParser.WhileStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}

		#endregion

		#region VisitForStmt Tests
		[TestMethod()]
		[DataRow("for(int a = 1, a < 5, a += 1) {}")]
		[DataRow("for(int a = 1, a < 5, a += 1) a")]
		[DataRow("for(int a = 1, a, a += 1) {}")]
		public void VisitForStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ForStmtNode node = (ForStmtNode)aSTVisitors.VisitForStmt((JanCParser.ForStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.ForStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("for(int a = 1, a < 5, a += 1) {}", typeof(VarDeclNode))]
		[DataRow("for(int a = 1, a < 5, a += 1) a", typeof(VarDeclNode))]
		[DataRow("for(int a = 1, a, a += 1) {}", typeof(VarDeclNode))]
		public void VisitForStmt_InitTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ForStmtNode node = (ForStmtNode)aSTVisitors.VisitForStmt((JanCParser.ForStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Init, expectedType);
		}
		[TestMethod()]
		[DataRow("for(int a = 1, a < 5, a += 1) {}", typeof(BinaryExprNode))]
		[DataRow("for(int a = 1, a < 5, a += 1) a", typeof(BinaryExprNode))]
		[DataRow("for(int a = 1, a, a += 1) {}", typeof(IdentifierExpr))]
		public void VisitForStmt_ConditionTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ForStmtNode node = (ForStmtNode)aSTVisitors.VisitForStmt((JanCParser.ForStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Condition, expectedType);
		}
		[TestMethod()]
		[DataRow("for(int a = 1, a < 5, a += 1) {}", typeof(AssignStmtNode))]
		[DataRow("for(int a = 1, a < 5, a += 1) a", typeof(AssignStmtNode))]
		[DataRow("for(int a = 1, a, a) {}", typeof(IdentifierExpr))]
		public void VisitForStmt_UpdateTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ForStmtNode node = (ForStmtNode)aSTVisitors.VisitForStmt((JanCParser.ForStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Update, expectedType);
		}
		[TestMethod()]
		[DataRow("for(int a = 1, a < 5, a += 1) {}", typeof(BlockNode))]
		[DataRow("for(int a = 1, a < 5, a += 1) a", typeof(IdentifierExpr))]
		[DataRow("for(int a = 1, a, a) {}", typeof(BlockNode))]
		public void VisitForStmt_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			ForStmtNode node = (ForStmtNode)aSTVisitors.VisitForStmt((JanCParser.ForStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}

		#endregion

		#region VisitSwitchStmt Tests
		[TestMethod()]
		[DataRow("switch(a) { \n\r 0 1 += 1 }")]
		[DataRow("switch(a) { \n\r 0 1 += 1 \n\r 0 1 += 1 }")]
		[DataRow("switch(a) { \n\r 0 1 += 1 } else { a += 1 }")]
		[DataRow("switch(a) { \n\r 0 1 += 1 } else a")]
		public void VisitSwitchStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchStmtNode node = (SwitchStmtNode)aSTVisitors.VisitSwitchStmt((JanCParser.SwitchStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.SwitchStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("switch(a) { \n\r 0 1 += 1 }", typeof(IdentifierExpr))]
		[DataRow("switch(func()) { \n\r 0 1 += 1 \n\r 0 1 += 1 }", typeof(CallExprNode))]
		[DataRow("switch(1) { \n\r 0 1 += 1 } else { a += 1 }", typeof(IntLiteralNode))]
		public void VisitSwitchStmt_ValueTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchStmtNode node = (SwitchStmtNode)aSTVisitors.VisitSwitchStmt((JanCParser.SwitchStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, expectedType);
		}
		[TestMethod()]
		[DataRow("switch(a) {  }")]
		[DataRow("switch(a) {  } else { a += 1 }")]
		public void VisitSwitchStmt_NoCasesTypeTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchStmtNode node = (SwitchStmtNode)aSTVisitors.VisitSwitchStmt((JanCParser.SwitchStmtContext)cst);

			// ASSERT
			Assert.AreEqual(0, node.Cases.Count);
		}
		[TestMethod()]
		[DataRow("switch(a) { \n\r 0 1 += 1 }", typeof(AssignStmtNode))]
		[DataRow("switch(a) { \n\r 0 func() \n\r 0 1 += 1 }", typeof(CallExprNode))]
		[DataRow("switch(a) { \n\r 0 1 += 1 } else { a += 1 }", typeof(AssignStmtNode))]
		public void VisitSwitchStmt_CaseTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchStmtNode node = (SwitchStmtNode)aSTVisitors.VisitSwitchStmt((JanCParser.SwitchStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Cases[0].Body, expectedType);
		}
		[TestMethod()]
		[DataRow("switch(a) { \n\r 0 1 += 1 \n\r 1 1 += 1 }", typeof(AssignStmtNode), typeof(AssignStmtNode))]
		[DataRow("switch(a) { \n\r 0 func() \n\r 1 1 += 1 }", typeof(CallExprNode), typeof(AssignStmtNode))]
		[DataRow("switch(a) { \n\r 0 1 += 1 \n\r 1 { func() } } else { a += 1 }", typeof(AssignStmtNode), typeof(BlockNode))]
		public void VisitSwitchStmt_CasesTypeTest(string input, Type firstCaseExpectedType, Type secondCaseExpectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchStmtNode node = (SwitchStmtNode)aSTVisitors.VisitSwitchStmt((JanCParser.SwitchStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Cases[0].Body, firstCaseExpectedType);
			Assert.IsInstanceOfType(node.Cases[1].Body, secondCaseExpectedType);
		}
		[TestMethod()]
		[DataRow("switch(a) { \n\r 0 1 += 1 \n\r 1 1 += 1 }", null)]
		[DataRow("switch(a) { \n\r 0 func() \n\r 1 1 += 1 } else {}", typeof(BlockNode))]
		[DataRow("switch(a) { \n\r 0 1 += 1 \n\r 1 { func() } } else a = 5", typeof(AssignStmtNode))]
		public void VisitSwitchStmt_DefaultCaseTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchStmtNode node = (SwitchStmtNode)aSTVisitors.VisitSwitchStmt((JanCParser.SwitchStmtContext)cst);

			// ASSERT
			if (expectedType != null)
				Assert.IsInstanceOfType(node.DefaultCase, expectedType);
			else
				Assert.AreEqual(expectedType, node.DefaultCase);
		}
		#endregion

		#region VisitSwitchCaseStmt
		[TestMethod()]
		[DataRow("0 1 += 1")]
		[DataRow("6 a")]
		public void VisitSwitchCaseStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).switchCaseStmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchCaseStmtNode node = (SwitchCaseStmtNode)aSTVisitors.VisitSwitchCaseStmt((JanCParser.SwitchCaseStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.SwitchCaseStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("0 1 += 1", typeof(IntLiteralNode))]
		[DataRow("6 a", typeof(IntLiteralNode))]
		[DataRow("b a", typeof(IdentifierExpr))]
		public void VisitSwitchCaseStmt_IdentifierTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).switchCaseStmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchCaseStmtNode node = (SwitchCaseStmtNode)aSTVisitors.VisitSwitchCaseStmt((JanCParser.SwitchCaseStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, expectedType);
		}
		[TestMethod()]
		[DataRow("0 1 += 1", typeof(AssignStmtNode))]
		[DataRow("6 a", typeof(IdentifierExpr))]
		[DataRow("1 { func() }", typeof(BlockNode))]
		public void VisitSwitchCaseStmt_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).switchCaseStmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			SwitchCaseStmtNode node = (SwitchCaseStmtNode)aSTVisitors.VisitSwitchCaseStmt((JanCParser.SwitchCaseStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
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
