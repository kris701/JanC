using ASTGenerator;
using CSTGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;

namespace IntegrationTests
{
    [TestClass]
    public class CSTGenerator_To_ASTGenerator
    {
		[TestMethod]
		public void CanGenerateSimpleASTTree() {
			// ARRANGE
			ICSTBuilder cSTBuilder = new CSTBuilder();
			IASTBuilder aSTBuilder = new ASTBuilder();
			cSTBuilder = cSTBuilder.ReadSource("int a = 5").BuildCST();

			// ACT
			aSTBuilder = aSTBuilder.ReadSource(cSTBuilder.CSTTree).BuildAST();

			// ASSERT
			Assert.IsInstanceOfType(aSTBuilder.ASTTree, typeof(GlobalScopeNode));
			Assert.AreEqual(1, ((GlobalScopeNode)aSTBuilder.ASTTree).Children.Count);
		}

		[TestMethod]
		public void CanChainGenerators() {
			// ARRANGE
			ICSTBuilder cSTBuilder = new CSTBuilder();
			IASTBuilder aSTBuilder = new ASTBuilder();

			// ACT
			aSTBuilder = aSTBuilder.ReadSource(cSTBuilder.ReadSource("int a = 5").BuildCST().CSTTree).BuildAST();

			// ASSERT
			Assert.IsInstanceOfType(aSTBuilder.ASTTree, typeof(GlobalScopeNode));
			Assert.AreEqual(1, ((GlobalScopeNode)aSTBuilder.ASTTree).Children.Count);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfParserErrors() {
			// ARRANGE
			ICSTBuilder cSTBuilder = new CSTBuilder();
			IASTBuilder aSTBuilder = new ASTBuilder();

			// ACT
			aSTBuilder.ReadSource(cSTBuilder.ReadSource("int a = {}").BuildCST().CSTTree).BuildAST();
		}
	}
}
