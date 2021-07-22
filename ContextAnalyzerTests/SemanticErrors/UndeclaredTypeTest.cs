using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class UndeclaredTypeTest {
		#region Constructor
		[TestMethod]
		public void UndeclaredTypeCorrectlySetsType() {
			// arrange
			ITypeLiteral typeLiteralNode = TypeNode.Bool;
			UndeclaredType undeclaredType;

			// act
			undeclaredType = new UndeclaredType(typeLiteralNode);

			// assert
			Assert.AreEqual(typeLiteralNode, undeclaredType.Type);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void UndeclaredSymbolThrowsOnNullIndentifierContext() {
			// arrange
			UndeclaredType undeclaredType = new UndeclaredType(
				TypeNode.Bool);

			// act
			undeclaredType.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			UndeclaredType undeclaredType = new UndeclaredType(
				TypeNode.Bool);

			// act
			string result = undeclaredType.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
