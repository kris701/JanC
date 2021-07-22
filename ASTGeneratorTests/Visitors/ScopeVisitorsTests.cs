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
	public class ScopeVisitorsTests {
		#region VisitEveryTaskNode Tests
		[TestMethod()]
		[DataRow("every 100 {}")]
		[DataRow("every 100 a")]
		[DataRow("every 1 func()")]
		public void VisitEveryTaskNode_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			EveryTaskNode node = (EveryTaskNode)aSTVisitors.VisitEveryTaskNode((JanCParser.EveryTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.EveryTaskNodeContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("every 100 {}", "100")]
		[DataRow("every 100 a", "100")]
		[DataRow("every 1 func()", "1")]
		public void VisitEveryTaskNode_DelayTest(string input, string expectedDelay) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			EveryTaskNode node = (EveryTaskNode)aSTVisitors.VisitEveryTaskNode((JanCParser.EveryTaskNodeContext)cst);

			// ASSERT
			Assert.AreEqual(expectedDelay, node.Delay);
		}
		[TestMethod()]
		[DataRow("every 100 {}", typeof(BlockNode))]
		[DataRow("every 100 a", typeof(IdentifierExpr))]
		[DataRow("every 1 func()", typeof(CallExprNode))]
		public void VisitEveryTaskNode_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			EveryTaskNode node = (EveryTaskNode)aSTVisitors.VisitEveryTaskNode((JanCParser.EveryTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}

		#endregion

		#region VisitOnTaskNode Tests
		[TestMethod()]
		[DataRow("on 1 {}")]
		[DataRow("on a == 2 a")]
		[DataRow("on (a) func()")]
		public void VisitOnTaskNode_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			OnTaskNode node = (OnTaskNode)aSTVisitors.VisitOnTaskNode((JanCParser.OnTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.OnTaskNodeContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("on 1 {}", typeof(IntLiteralNode))]
		[DataRow("on a == 2 a", typeof(BinaryExprNode))]
		[DataRow("on (a) func()", typeof(IdentifierExpr))]
		public void VisitOnTaskNode_ConditionTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			OnTaskNode node = (OnTaskNode)aSTVisitors.VisitOnTaskNode((JanCParser.OnTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Condition, expectedType);
		}
		[TestMethod()]
		[DataRow("on 1 {}", typeof(BlockNode))]
		[DataRow("on a == 2 a", typeof(IdentifierExpr))]
		[DataRow("on (a) func()", typeof(CallExprNode))]
		public void VisitOnTaskNode_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			OnTaskNode node = (OnTaskNode)aSTVisitors.VisitOnTaskNode((JanCParser.OnTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}

		#endregion

		#region VisitOnceTaskNode Tests
		[TestMethod()]
		[DataRow("once {}")]
		[DataRow("once a")]
		[DataRow("once func()")]
		public void VisitOnceTaskNode_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			OnceTaskNode node = (OnceTaskNode)aSTVisitors.VisitOnceTaskNode((JanCParser.OnceTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.OnceTaskNodeContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("once {}", typeof(BlockNode))]
		[DataRow("once a", typeof(IdentifierExpr))]
		[DataRow("once func()", typeof(CallExprNode))]
		public void VisitOnceTaskNode_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			OnceTaskNode node = (OnceTaskNode)aSTVisitors.VisitOnceTaskNode((JanCParser.OnceTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}

		#endregion

		#region VisitIdleTaskNode Tests
		[TestMethod()]
		[DataRow("idle {}")]
		[DataRow("idle a")]
		[DataRow("idle func()")]
		public void VisitIdleTaskNode_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IdleTaskNode node = (IdleTaskNode)aSTVisitors.VisitIdleTaskNode((JanCParser.IdleTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.IdleTaskNodeContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("idle {}", typeof(BlockNode))]
		[DataRow("idle a", typeof(IdentifierExpr))]
		[DataRow("idle func()", typeof(CallExprNode))]
		public void VisitIdleTaskNode_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IdleTaskNode node = (IdleTaskNode)aSTVisitors.VisitIdleTaskNode((JanCParser.IdleTaskNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}

		#endregion

		#region VisitCriticalStmt Tests
		[TestMethod()]
		[DataRow("critical {}")]
		[DataRow("critical a")]
		[DataRow("critical func()")]
		public void VisitCriticalStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CriticalNode node = (CriticalNode)aSTVisitors.VisitCriticalStmt((JanCParser.CriticalStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.GenericContext, typeof(JanCParser.CriticalStmtContext));
			Assert.AreEqual(cst, node.GenericContext);
		}
		[TestMethod()]
		[DataRow("critical {}", typeof(BlockNode))]
		[DataRow("critical a", typeof(IdentifierExpr))]
		[DataRow("critical func()", typeof(CallExprNode))]
		public void VisitCriticalStmt_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			CriticalNode node = (CriticalNode)aSTVisitors.VisitCriticalStmt((JanCParser.CriticalStmtContext)cst);

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
