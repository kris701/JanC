using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Nodes;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class CanOnlyCallFunctionTest {
		#region Constructor
		[TestMethod]
		public void CanOnlyCallFunctionCorrectlySetsCallExpr() {
			CallExprNode callExpr = new CallExprNode(null, null, null);

			var expectedFunctionIdentifier = new CanOnlyCallFunction(callExpr);

			Assert.AreEqual(callExpr, expectedFunctionIdentifier.Call);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullIdentifierContext() {
			// arrange
			CanOnlyCallFunction expectedFunctionIdentifier = new CanOnlyCallFunction(
				new CallExprNode(null, null, null));

			// act
			expectedFunctionIdentifier.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullGenericContext() {
			// arrange
			CanOnlyCallFunction expectedFunctionIdentifier = new CanOnlyCallFunction(
				new CallExprNode(null, null, null));

			// act
			expectedFunctionIdentifier.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullNameToken() {
			// arrange
			CanOnlyCallFunction expectedFunctionIdentifier = new CanOnlyCallFunction(
				new CallExprNode(null, null, null));

			// act
			expectedFunctionIdentifier.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			CanOnlyCallFunction expectedFunctionIdentifier = new CanOnlyCallFunction(
				new CallExprNode(null, null, null));

			// act
			string result = expectedFunctionIdentifier.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
