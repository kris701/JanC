using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class ReturnTypeMismatchTest {
		#region Constructor
		[TestMethod]
		public void ReturnTypeMismatchCorrectlySetsReturnStmt() {
			// arrange
			ReturnStmtNode returnStmtNode = new ReturnStmtNode(null, null);
			ReturnTypeMismatch returnTypeMismatch;

			// act
			returnTypeMismatch = new ReturnTypeMismatch(returnStmtNode, null);

			// assert
			Assert.AreEqual(returnStmtNode, returnTypeMismatch.ReturnStmt);
		}

		[TestMethod]
		public void ReturnTypeMismatchCorrectlySetsFuncDecl() {
			// arrange
			FuncDeclNode funcDeclNode = new FuncDeclNode(null, null, null, null, null);
			ReturnTypeMismatch returnTypeMismatch;

			// act
			returnTypeMismatch = new ReturnTypeMismatch(null, funcDeclNode);

			// assert
			Assert.AreEqual(funcDeclNode, returnTypeMismatch.FuncDecl);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullReturnStmt() {
			// arrange
			ReturnTypeMismatch returnTypeMismatch = new ReturnTypeMismatch(
				new ReturnStmtNode(null, null),
				new FuncDeclNode(new JanCParser.FuncDeclContext(new JanCParser.DeclContext()), null, null, null, null));

			// act
			returnTypeMismatch.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullFuncDecl() {
			// arrange
			ReturnTypeMismatch returnTypeMismatch = new ReturnTypeMismatch(
				new ReturnStmtNode(new JanCParser.ReturnStmtContext(new JanCParser.StmtContext()), null),
				new FuncDeclNode(null, null, null, null, null));

			// act
			returnTypeMismatch.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			ReturnTypeMismatch returnTypeMismatch = new ReturnTypeMismatch(
				new ReturnStmtNode(null, null),
				new FuncDeclNode(null, null, TypeNode.Bool, null, null));

			// act
			string result = returnTypeMismatch.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
