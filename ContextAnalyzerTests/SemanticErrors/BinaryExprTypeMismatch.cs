using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class BinaryExprTypeMismatchTest {
		#region Constructor
		[TestMethod]
		public void BinaryExprTypeMismatchCorrectlySetsLeftType() {
			// arrange
			ITypeLiteral typeLiteralNode = TypeNode.Bool;
			BinaryExprTypeMismatch binaryExprTypeMismatch;

			// act
			binaryExprTypeMismatch = new BinaryExprTypeMismatch(typeLiteralNode, null, null, null);

			// assert
			Assert.AreEqual(typeLiteralNode, binaryExprTypeMismatch.LeftType);
		}

		[TestMethod]
		public void BinaryExprTypeMismatchCorrectlySetsOperator() {
			// arrange
			string testString = "";
			BinaryExprTypeMismatch binaryExprTypeMismatch;

			// act
			binaryExprTypeMismatch = new BinaryExprTypeMismatch(null, testString, null, null);

			// assert
			Assert.AreEqual(testString, binaryExprTypeMismatch.Operator);
		}

		[TestMethod]
		public void BinaryExprTypeMismatchCorrectlySetsRightType() {
			// arrange
			ITypeLiteral typeLiteralNode = TypeNode.Bool;
			BinaryExprTypeMismatch binaryExprTypeMismatch;

			// act
			binaryExprTypeMismatch = new BinaryExprTypeMismatch(null, null, typeLiteralNode, null);

			// assert
			Assert.AreEqual(typeLiteralNode, binaryExprTypeMismatch.RightType);
		}

		[TestMethod]
		public void BinaryExprTypeMismatchCorrectlySetsContext() {
			// arrange
			JanCParser.BinaryExprContext binaryExprContext = new JanCParser.BinaryExprContext(new JanCParser.ExprContext());
			BinaryExprTypeMismatch binaryExprTypeMismatch;

			// act
			binaryExprTypeMismatch = new BinaryExprTypeMismatch(null, null, null, binaryExprContext);

			// assert
			Assert.AreEqual(binaryExprContext, binaryExprTypeMismatch.Context);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullContext() {
			// arrange
			BinaryExprTypeMismatch binaryExprTypeMismatch = new BinaryExprTypeMismatch(null, null, null, null);

			// act
			binaryExprTypeMismatch.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			BinaryExprTypeMismatch binaryExprTypeMismatch = new BinaryExprTypeMismatch(
				TypeNode.Bool,
				"",
				TypeNode.Bool, null);

			// act
			string result = binaryExprTypeMismatch.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
