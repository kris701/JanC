using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class InvalidVoidUsageTest {
		#region Constructor
		[TestMethod]
		public void UndeclaredTypeCorrectlySetsType() {
			// arrange
			ITypeLiteral typeLiteralNode = TypeNode.Bool;
			InvalidVoidUsage variableIsVoid;

			// act
			variableIsVoid = new InvalidVoidUsage(typeLiteralNode);

			// assert
			Assert.AreEqual(typeLiteralNode, variableIsVoid.Type);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void UndeclaredSymbolThrowsOnNullIndentifierContext() {
			// arrange
			InvalidVoidUsage variableIsVoid = new InvalidVoidUsage(
				TypeNode.Bool);

			// act
			variableIsVoid.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			InvalidVoidUsage variableIsVoid = new InvalidVoidUsage(null);

			// act
			string result = variableIsVoid.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
