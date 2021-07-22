using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class AssignmentTypeMismatchTest {
		#region Constructor
		[TestMethod]
		public void AssignmentTypeMismatchCorrectlySetsAssignStmt() {
			var assignStmt = new AssignStmtNode(null, null, AssignOperator.Assign, null);
			var assignmentTypeMismatch = new AssignmentTypeMismatch(assignStmt);
			Assert.AreEqual(assignStmt, assignmentTypeMismatch.AssignStmt);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullAssignStmtContext() {
			var assignmentTypeMismatch =
				new AssignmentTypeMismatch(
					new AssignStmtNode(null, null, AssignOperator.Assign, null));
			assignmentTypeMismatch.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {

			var location = new IdentifierExpr(null, "varName", TypeNode.Bool);

			var assignment = new AssignStmtNode(
				null,
				location,
				AssignOperator.Assign,
				new BinaryExprNode(null, null, null, null)
			);

			AssignmentTypeMismatch assignmentTypeMismatch =
				new AssignmentTypeMismatch(assignment);

			// act
			string result = assignmentTypeMismatch.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
