using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class SymbolAlreadyDeclaredTest {
		#region Constructor
		[TestMethod]
		public void SymbolAlreadyDeclaredCorrectlySetsCurrent() {
			// arrange
			ITypedDecl typedDecl = new VarDeclNode(null, null, null, null, false);
			SymbolAlreadyDeclared symbolAlreadyDeclared;

			// act
			symbolAlreadyDeclared = new SymbolAlreadyDeclared(typedDecl, null);

			// assert
			Assert.AreEqual(typedDecl, symbolAlreadyDeclared.Current);
		}

		[TestMethod]
		public void SymbolAlreadyDeclaredCorrectlySetsPrev() {
			// arrange
			ITypedDecl typedDecl = new VarDeclNode(null, null, null, null, false);
			SymbolAlreadyDeclared symbolAlreadyDeclared;

			// act
			symbolAlreadyDeclared = new SymbolAlreadyDeclared(null, typedDecl);

			// assert
			Assert.AreEqual(typedDecl, symbolAlreadyDeclared.Prev);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullCurrentGenericContext() {
			// arrange
			SymbolAlreadyDeclared symbolAlreadyDeclared = new SymbolAlreadyDeclared(
				new VarDeclNode(null, "", null, null, false),
				new VarDeclNode(new JanCParser.VarDeclContext(null, 0), "", null, null, false));

			// act
			symbolAlreadyDeclared.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullPrevGenericContext() {
			// arrange
			SymbolAlreadyDeclared symbolAlreadyDeclared = new SymbolAlreadyDeclared(
				new VarDeclNode(new JanCParser.VarDeclContext(null, 0), "", null, null, false),
				new VarDeclNode(null, "", null, null, false));

			// act
			symbolAlreadyDeclared.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullCurrentNameToken() {
			// arrange
			SymbolAlreadyDeclared symbolAlreadyDeclared = new SymbolAlreadyDeclared(
				new VarDeclNode(new JanCParser.VarDeclContext(null, 0), null, null, null, false),
				new VarDeclNode(new JanCParser.VarDeclContext(null, 0), "", null, null, false));

			// act
			symbolAlreadyDeclared.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullPrevNameToken() {
			// arrange
			SymbolAlreadyDeclared symbolAlreadyDeclared = new SymbolAlreadyDeclared(
				new VarDeclNode(new JanCParser.VarDeclContext(null, 0), "", null, null, false),
				new VarDeclNode(new JanCParser.VarDeclContext(null, 0), null, null, null, false));

			// act
			symbolAlreadyDeclared.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			SymbolAlreadyDeclared symbolAlreadyDeclared = new SymbolAlreadyDeclared(
				new VarDeclNode(null, null, TypeNode.Bool, null, false),
				null);

			// act
			string result = symbolAlreadyDeclared.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
