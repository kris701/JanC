using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class UndeclaredSymbolTest {
		#region Constructor
		[TestMethod]
		public void UndeclaredSymbolCorrectlySetsIdentifier() {
			// arrange
			IdentifierExpr identifier = new IdentifierExpr(null, null);
			UndeclaredSymbol undeclaredSymbol;

			// act
			undeclaredSymbol = new UndeclaredSymbol(identifier);

			// assert
			Assert.AreEqual(identifier, undeclaredSymbol.Identifier);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void UndeclaredSymbolThrowsOnNullIndentifierContext() {
			// arrange
			UndeclaredSymbol undeclaredSymbol = new UndeclaredSymbol(
				new IdentifierExpr(null, null));

			// act
			undeclaredSymbol.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			UndeclaredSymbol undeclaredSymbol = new UndeclaredSymbol(
				new IdentifierExpr(null, ""));

			// act
			string result = undeclaredSymbol.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
