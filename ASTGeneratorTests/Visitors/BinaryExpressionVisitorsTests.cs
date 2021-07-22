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
	public class BinaryExpressionVisitorsTests {
		#region VisitBinaryExpr Tests
		[TestMethod()]
		[TestCategory("SlowTest")]
		[DataRow("a + b")]
		[DataRow("a - b")]
		[DataRow("a + 5")]
		[DataRow("-5 + 5")]

		public void VisitBinaryExpr_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BinaryExprNode node = (BinaryExprNode)aSTVisitors.VisitBinaryExpr((JanCParser.BinaryExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.BinaryExprContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("a + b", typeof(IdentifierExpr), typeof(IdentifierExpr))]
		[DataRow("a - b", typeof(IdentifierExpr), typeof(IdentifierExpr))]
		[DataRow("a + 5", typeof(IdentifierExpr), typeof(IntLiteralNode))]
		[DataRow("-5 + 5", typeof(NegateNode), typeof(IntLiteralNode))]

		public void VisitBinaryExpr_TypeTest(string input, Type leftType, Type rightType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BinaryExprNode node = (BinaryExprNode)aSTVisitors.VisitBinaryExpr((JanCParser.BinaryExprContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Left, leftType);
			Assert.IsInstanceOfType(node.Right, rightType);
		}
		[TestMethod()]
		[DataRow("a + b", "+")]
		[DataRow("a - b", "-")]
		[DataRow("a * 5", "*")]
		[DataRow("-5 / 5", "/")]

		public void VisitBinaryExpr_OperatorTest(string input, string expectedOp) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).expr();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BinaryExprNode node = (BinaryExprNode)aSTVisitors.VisitBinaryExpr((JanCParser.BinaryExprContext)cst);

			// ASSERT
			Assert.AreEqual(expectedOp, node.Operator);
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
