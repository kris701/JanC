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
	public class UnaryVisitorsTests {
		#region VisitUnaryExpr Tests
		[TestMethod()]
		[DataRow("-1")]
		public void VisitUnaryExpr_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IUnary node = (IUnary)aSTVisitors.VisitUnaryExpr((JanCParser.UnaryExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.GenericContext, typeof(JanCParser.UnaryExprContext));
			Assert.AreEqual(cst, node.GenericContext);
		}
		[TestMethod()]
		[DataRow("-1", typeof(NegateNode))]
		[DataRow("+1", typeof(IntLiteralNode))]
		public void VisitUnaryExpr_OpTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IASTNode node = aSTVisitors.Visit(cst);

			// ASSERT
			Assert.IsInstanceOfType(node, expectedType);
		}
		[TestMethod()]
		public void VisitUnaryExpr_PlusOpGivesLiteralTest() {
			// ARRANGE
			var cst = (ParserRuleContext)getParser("-1").expr();
			ITokenFactory tokenFactory = new CommonTokenFactory();
			JanCParser.UnaryExprContext context = (JanCParser.UnaryExprContext)cst;
			context.op = tokenFactory.Create(JanCLexer.PLUS, "+");
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IASTNode node = aSTVisitors.VisitUnaryExpr(context);

			// ASSERT
			Assert.IsInstanceOfType(node, typeof(IntLiteralNode));
		}
		[TestMethod()]
		[ExpectedException(typeof(NotSupportedException))]
		public void VisitUnaryExpr_AnyOtherOpThrowsExceptionTest() {
			// ARRANGE
			var cst = (ParserRuleContext)getParser("-1").expr();
			ITokenFactory tokenFactory = new CommonTokenFactory();
			JanCParser.UnaryExprContext context = (JanCParser.UnaryExprContext)cst;
			context.op = tokenFactory.Create(0, "^");
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IASTNode node = aSTVisitors.VisitUnaryExpr(context);

			// ASSERT
		}

		#endregion

		#region VisitNotNode Tests
		[TestMethod()]
		[DataRow("!1")]
		[DataRow("!(1 + 2)")]
		public void VisitNotNode_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			NotNode node = (NotNode)aSTVisitors.VisitNotNode((JanCParser.NotNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.NotNodeContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("!1", typeof(IntLiteralNode))]
		[DataRow("!(1 + 2)", typeof(GroupingExprNode))]
		[DataRow("!func()", typeof(CallExprNode))]
		public void VisitNotNode_ValueTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			NotNode node = (NotNode)aSTVisitors.VisitNotNode((JanCParser.NotNodeContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, expectedType);
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
