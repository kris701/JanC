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
	public class DeclarationVisitorsTests {
		#region VisitVarDecl Tests
		[TestMethod()]
		[DataRow("int a = 5")]
		[DataRow("str a = \"5\"")]
		[DataRow("float a = 5")]
		public void VisitVarDecl_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).varDecl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			VarDeclNode node = (VarDeclNode)aSTVisitors.VisitVarDecl((JanCParser.VarDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.VarDeclContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("int a = 5", PrimitiveTypeNode.Types.Integer)]
		[DataRow("str a = \"5\"", PrimitiveTypeNode.Types.String)]
		[DataRow("float a = 5", PrimitiveTypeNode.Types.Float)]
		public void VisitVarDecl_BasetypeTest(string input, PrimitiveTypeNode.Types typeType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).varDecl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			VarDeclNode node = (VarDeclNode)aSTVisitors.VisitVarDecl((JanCParser.VarDeclContext)cst);

			// ASSERT
			Assert.AreEqual(typeType, ((PrimitiveTypeNode)node.Type).Type);
		}
		[TestMethod()]
		[DataRow("int a = 5", typeof(IntLiteralNode))]
		[DataRow("str a = \"5\"", typeof(StringLiteralNode))]
		[DataRow("float a = 5.0", typeof(FloatLiteralNode))]
		public void VisitVarDecl_LiteralTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).varDecl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			VarDeclNode node = (VarDeclNode)aSTVisitors.VisitVarDecl((JanCParser.VarDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Value, expectedType);
		}

		#endregion

		#region VisitBlockStmt Tests
		[TestMethod()]
		[DataRow("{}")]
		[DataRow("{ a }")]
		[DataRow("{ a \n\r b }")]
		public void VisitBlockStmt_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BlockNode node = (BlockNode)aSTVisitors.VisitBlockStmt((JanCParser.BlockStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.BlockStmtContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("{}")]
		[DataRow("{                }")]
		[DataRow("{ \n\r }")]
		public void VisitBlockStmt_NoChildrenTypeTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BlockNode node = (BlockNode)aSTVisitors.VisitBlockStmt((JanCParser.BlockStmtContext)cst);

			// ASSERT
			Assert.AreEqual(0, node.Children.Count);
		}
		[TestMethod()]
		[DataRow("{ a }", typeof(IdentifierExpr))]
		[DataRow("{ 5 \n\r b }", typeof(IntLiteralNode))]
		public void VisitBlockStmt_ChildTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BlockNode node = (BlockNode)aSTVisitors.VisitBlockStmt((JanCParser.BlockStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Children[0], expectedType);
		}
		[TestMethod()]
		[DataRow("{ 5 \n\r b }", typeof(IntLiteralNode), typeof(IdentifierExpr))]
		[DataRow("{ 5 += 1 \n\r b }", typeof(AssignStmtNode), typeof(IdentifierExpr))]
		[DataRow("{ func() \n\r b -= 5 }", typeof(CallExprNode), typeof(AssignStmtNode))]
		public void VisitBlockStmt_ChildrenTypeTest(string input, Type firstChildExpectedType, Type secondChildExpectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).stmt();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			BlockNode node = (BlockNode)aSTVisitors.VisitBlockStmt((JanCParser.BlockStmtContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Children[0], firstChildExpectedType);
			Assert.IsInstanceOfType(node.Children[1], secondChildExpectedType);
		}
		#endregion

		#region VisitFuncDecl Tests
		[TestMethod()]
		[DataRow("void a(){}")]
		[DataRow("void a(int a){}")]
		[DataRow("int a(int a, int b){}")]
		[DataRow("void a() a")]
		public void VisitFuncDecl_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FuncDeclNode node = (FuncDeclNode)aSTVisitors.VisitFuncDecl((JanCParser.FuncDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.FuncDeclContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("void a(){}", PrimitiveTypeNode.Types.Void)]
		[DataRow("bool a(int a){}", PrimitiveTypeNode.Types.Bool)]
		[DataRow("int a(int a, int b){}", PrimitiveTypeNode.Types.Integer)]
		[DataRow("float a() a", PrimitiveTypeNode.Types.Float)]
		public void VisitFuncDecl_TypeTest(string input, PrimitiveTypeNode.Types functionType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FuncDeclNode node = (FuncDeclNode)aSTVisitors.VisitFuncDecl((JanCParser.FuncDeclContext)cst);

			// ASSERT
			Assert.AreEqual(functionType, ((PrimitiveTypeNode)node.ReturnType).Type);
		}
		[TestMethod()]
		[DataRow("void a(){}")]
		[DataRow("bool a(){}")]
		[DataRow("int a(){}")]
		public void VisitFuncDecl_NoParametersTypeTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FuncDeclNode node = (FuncDeclNode)aSTVisitors.VisitFuncDecl((JanCParser.FuncDeclContext)cst);

			// ASSERT
			Assert.AreEqual(0, node.Parameters.Count);
		}
		[TestMethod()]
		[DataRow("bool a(int a){}", typeof(VarDeclNode))]
		[DataRow("int a(int a, int b){}", typeof(VarDeclNode))]
		public void VisitFuncDecl_ParameterTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FuncDeclNode node = (FuncDeclNode)aSTVisitors.VisitFuncDecl((JanCParser.FuncDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Parameters[0], expectedType);
		}
		[TestMethod()]
		[DataRow("void a(){}", null, null)]
		[DataRow("bool a(int a){}", typeof(VarDeclNode), null)]
		[DataRow("int a(int a, int b){}", typeof(VarDeclNode), typeof(VarDeclNode))]
		public void VisitFuncDecl_ParametersTypeTest(string input, Type firstParamExpectedType, Type secondParamExpectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FuncDeclNode node = (FuncDeclNode)aSTVisitors.VisitFuncDecl((JanCParser.FuncDeclContext)cst);

			// ASSERT
			if (firstParamExpectedType != null)
				Assert.IsInstanceOfType(node.Parameters[0], firstParamExpectedType);
			if (secondParamExpectedType != null)
				Assert.IsInstanceOfType(node.Parameters[1], secondParamExpectedType);
		}
		[TestMethod()]
		[DataRow("void a(){}", typeof(BlockNode))]
		[DataRow("bool a() return a", typeof(ReturnStmtNode))]
		[DataRow("int a() a", typeof(IdentifierExpr))]
		public void VisitFuncDecl_BodyTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			FuncDeclNode node = (FuncDeclNode)aSTVisitors.VisitFuncDecl((JanCParser.FuncDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Body, expectedType);
		}


		#endregion

		#region VisitStructDecl Tests
		[TestMethod()]
		[DataRow("struct test {  }")]
		[DataRow("struct test { \n\r int a }")]
		[DataRow("struct test { \n\r int a \n\r int b }")]
		public void VisitStructDecl_ContextTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructDeclNode node = (StructDeclNode)aSTVisitors.VisitStructDecl((JanCParser.StructDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Context, typeof(JanCParser.StructDeclContext));
			Assert.AreEqual(cst, node.Context);
		}
		[TestMethod()]
		[DataRow("struct test {  }", "test")]
		[DataRow("struct abc { \n\r int a }", "abc")]
		[DataRow("struct fancystructnamethatiswaytoolong { \n\r int a \n\r int b }", "fancystructnamethatiswaytoolong")]
		public void VisitStructDecl_NameTest(string input, string expectedName) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructDeclNode node = (StructDeclNode)aSTVisitors.VisitStructDecl((JanCParser.StructDeclContext)cst);

			// ASSERT
			Assert.AreEqual(expectedName, node.Name);
		}
		[TestMethod()]
		[DataRow("struct test {  }")]
		public void VisitStructDecl_NoMembersTypeTest(string input) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructDeclNode node = (StructDeclNode)aSTVisitors.VisitStructDecl((JanCParser.StructDeclContext)cst);

			// ASSERT
			Assert.AreEqual(0, node.Members.Count);
		}
		[TestMethod()]
		[DataRow("struct abc { \n\r int a }", typeof(VarDeclNode))]
		[DataRow("struct fancystructnamethatiswaytoolong { \n\r int a \n\r int b }", typeof(VarDeclNode))]
		public void VisitStructDecl_MemberTypeTest(string input, Type expectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructDeclNode node = (StructDeclNode)aSTVisitors.VisitStructDecl((JanCParser.StructDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Members[0], expectedType);
		}
		[TestMethod()]
		[DataRow("struct abc { \n\r int a \n\r int a }", typeof(VarDeclNode), typeof(VarDeclNode))]
		[DataRow("struct abc { \n\r int abaa \n\r str stringvalue }", typeof(VarDeclNode), typeof(VarDeclNode))]
		public void VisitStructDecl_MembersTypeTest(string input, Type firstMemberExpectedType, Type secondMemberExpectedType) {
			// ARRANGE
			var cst = (ParserRuleContext)getParser(input).decl();
			CSTVisitors aSTVisitors = new CSTVisitors();

			// ACT
			StructDeclNode node = (StructDeclNode)aSTVisitors.VisitStructDecl((JanCParser.StructDeclContext)cst);

			// ASSERT
			Assert.IsInstanceOfType(node.Members[0], firstMemberExpectedType);
			Assert.IsInstanceOfType(node.Members[1], secondMemberExpectedType);
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
