using ASTGenerator;
using ContextAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests {
	[TestClass]
	public class ASTGenerator_To_ContextAnalyzer {
		[TestMethod]
		public void CanAnalyzeSimpleASTTree() {
			// ARRANGE
			IASTBuilder aSTBuilder = new ASTBuilder().ReadSource(new JanCParser.CompileUnitContext(null, 0)).BuildAST();
			IContextAnalyzer contextAnalzer = new ContextAnalyzer.ContextAnalyzer();

			// ACT
			contextAnalzer = contextAnalzer.ReadSource(aSTBuilder.ASTTree).SetErrorListener(new Exceptions.Syntax.SemanticErrorListener()).SetPhases().DecorateAST();

			// ASSERT
			Assert.IsInstanceOfType(contextAnalzer.DecoratedAST, typeof(GlobalScopeNode));
			Assert.AreEqual(0, contextAnalzer.ErrorListener.Errors.Count);
			Assert.AreEqual(0, contextAnalzer.ErrorListener.Warnings.Count);
		}
		[TestMethod]
		public void CanChainGenerators() {
			// ARRANGE
			// ACT
			IContextAnalyzer contextAnalzer =
				new ContextAnalyzer.ContextAnalyzer().ReadSource(
					new ASTBuilder().ReadSource(new JanCParser.CompileUnitContext(null, 0)).BuildAST().ASTTree
				).SetErrorListener(new Exceptions.Syntax.SemanticErrorListener()).SetPhases().DecorateAST();

			// ASSERT
			Assert.IsInstanceOfType(contextAnalzer.DecoratedAST, typeof(GlobalScopeNode));
			Assert.AreEqual(0, contextAnalzer.ErrorListener.Errors.Count);
			Assert.AreEqual(0, contextAnalzer.ErrorListener.Warnings.Count);
		}
	}
}
