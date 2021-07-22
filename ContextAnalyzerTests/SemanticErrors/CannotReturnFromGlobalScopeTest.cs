using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class CannotReturnFromGlobalScopeTest {
		#region Constructor
		[TestMethod]
		public void CannotReturnFromGlobalScopeCorrectlySetsReturnStmt() {
			// arrange
			ReturnStmtNode returnStmtNode = new ReturnStmtNode(null, null);
			CannotReturnFromGlobalScope cannotReturnFromGlobalScope;

			// act
			cannotReturnFromGlobalScope = new CannotReturnFromGlobalScope(returnStmtNode);

			// assert
			Assert.AreEqual(returnStmtNode, cannotReturnFromGlobalScope.ReturnStmt);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullContext() {
			// arrange
			CannotReturnFromGlobalScope cannotReturnFromGlobalScope = new CannotReturnFromGlobalScope(
				new ReturnStmtNode(null, null));

			// act
			cannotReturnFromGlobalScope.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			CannotReturnFromGlobalScope cannotReturnFromGlobalScope = new CannotReturnFromGlobalScope(
				new ReturnStmtNode(null, null));

			// act
			string result = cannotReturnFromGlobalScope.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
