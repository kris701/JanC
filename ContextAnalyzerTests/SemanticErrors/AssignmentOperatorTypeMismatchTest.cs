using Nodes;
using Exceptions.Syntax.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	/*
	[TestClass]
	public class AssignmentOperatorTypeMismatchTest {
		#region Constructor
		[TestMethod]
		public void AssignmentOperatorTypeMismatchCorrectlySetsAssignStmtNode() {
			// arrange
			AssignStmtNode assignStmtNode = new AssignStmtNode(null, null, null, null);
			AssignmentOperatorTypeMismatch assignmentOperatorTypeMismatch;

			// act
			assignmentOperatorTypeMismatch = new AssignmentOperatorTypeMismatch(assignStmtNode, null);

			// assert
			Assert.AreEqual(assignStmtNode, assignmentOperatorTypeMismatch.AssignStmt);
		}

		[TestMethod]
		public void AssignmentOperatorTypeMismatchCorrectlySetsVarDeclNode() {
			// arrange
			VarDeclNode varDeclNode = new VarDeclNode(null, null, null, null);
			AssignmentOperatorTypeMismatch assignmentOperatorTypeMismatch;

			// act
			assignmentOperatorTypeMismatch = new AssignmentOperatorTypeMismatch(null, varDeclNode);

			// assert
			Assert.AreEqual(varDeclNode, assignmentOperatorTypeMismatch.VarDecl);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullAssignStmtContext() {
			// arrange
			AssignmentOperatorTypeMismatch assignmentOperatorTypeMismatch =
				new AssignmentOperatorTypeMismatch(
					new AssignStmtNode(null, null, null, null),
					new VarDeclNode(new JanCParser.VarDeclContext(null, 0), null, null, null));

			// act
			assignmentOperatorTypeMismatch.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullVarDeclContext() {
			// arrange
			AssignmentOperatorTypeMismatch assignmentOperatorTypeMismatch =
				new AssignmentOperatorTypeMismatch(
					new AssignStmtNode(new JanCParser.AssignStmtContext(new JanCParser.StmtContext()), null, null, null),
					new VarDeclNode(null, null, null, null));

			// act
			assignmentOperatorTypeMismatch.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			AssignmentOperatorTypeMismatch assignmentOperatorTypeMismatch =
				new AssignmentOperatorTypeMismatch(
					new AssignStmtNode(null, new IdentifierExpr(null, "", Bool), "", null),
					new VarDeclNode(null, null, null, null));

			// act
			string result = assignmentOperatorTypeMismatch.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}*/
}
