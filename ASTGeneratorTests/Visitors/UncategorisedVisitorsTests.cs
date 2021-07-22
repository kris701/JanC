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
	public class UncategorisedVisitorsTests {
		#region BuildASTTree Tests
		[TestMethod()]
		[DataRow("")]
		[DataRow("void func() a")]
		[DataRow("int a = 5")]
		public void BuildASTTree_CanCallTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).compileUnit();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IASTNode node = aSTVisitors.BuildASTTree(cst);

			// ASSERT
			Assert.IsNotNull(node);
		}
		#endregion

		#region VisitCompileUnit Tests
		[TestMethod()]
		[DataRow("")]
		[DataRow("void func() a")]
		[DataRow("int a = 5")]
		public void VisitCompileUnit_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).compileUnit();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			GlobalScopeNode node = (GlobalScopeNode)aSTVisitors.VisitCompileUnit((JanCParser.CompileUnitContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.GenericContext, typeof(JanCParser.CompileUnitContext));
			Assert.AreEqual(cst, node.GenericContext);
		}

		#endregion

		#region VisitUnit Tests
		[TestMethod()]
		[DataRow("void func() a", typeof(FuncDeclNode))]
		[DataRow("int a = 5", typeof(VarDeclNode))]
		public void VisitUnit_ContextTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).unit();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			IASTNode node = aSTVisitors.VisitUnit((JanCParser.UnitContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node, expectedType);
		}
		#endregion

		#region VisitTypeLiteral
		[TestMethod()]
		[DataRow("int")]
		[DataRow("str")]
		[DataRow("float")]
		[DataRow("bool")]
		[DataRow("void")]
		public void VisitTypeLiteral_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).typeLiteral();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			TypeNode node = (TypeNode)aSTVisitors.Visit((JanCParser.TypeLiteralContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.TypeLiteralContext));
			Assert.AreEqual(cst, node.Context);
		}

		[TestMethod()]
		[DataRow("int", PrimitiveTypeNode.Types.Integer)]
		[DataRow("str", PrimitiveTypeNode.Types.String)]
		[DataRow("float", PrimitiveTypeNode.Types.Float)]
		[DataRow("bool", PrimitiveTypeNode.Types.Bool)]
		[DataRow("void", PrimitiveTypeNode.Types.Void)]
		public void VisitTypeLiteral_PrimitiveTypeTest(string input, PrimitiveTypeNode.Types expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).typeLiteral();
			CSTVisitors astVisitors = new CSTVisitors();

			// ACT
			TypeNode node = (TypeNode)astVisitors.Visit((JanCParser.TypeLiteralContext)cst);

			// ASSERT
			Assert.AreEqual(expectedType, ((PrimitiveTypeNode)node).Type);
		}

		[DataRow("structname")]
		public void VisitTypeLiteral_UserDefinedTypeTest(string input, PrimitiveTypeNode.Types expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).typeLiteral();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			TypeNode node = (TypeNode)aSTVisitors.Visit((JanCParser.TypeLiteralContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node, typeof(UserDefinedTypeNode));
		}

		[TestMethod]
		public void VisitTypeLiteral_AccessedUserDefinedTypeTest() {
			var cst = (ParserRuleContext)getParser("module1.module2.structName").typeLiteral();
			CSTVisitors cstVisitors = new CSTVisitors();
			var node = (UserDefinedTypeNode)cstVisitors.Visit((JanCParser.TypeLiteralContext)cst);
			Assert.AreEqual(node.Name, "structName");
			Assert.IsTrue(node.ModuleNames.SequenceEqual(new List<string> { "module1", "module2" }));
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
