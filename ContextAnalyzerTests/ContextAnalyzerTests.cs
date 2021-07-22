using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContextAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodes;
using Tools;
using Exceptions.Syntax;
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;

namespace ContextAnalyzer.Tests {
	[TestClass()]
	public class ContextAnalyzerTests {

		#region Test Setup

		private IContextAnalyzer contextAnalyzer;

		[TestInitialize()]
		public void Setup() {
			contextAnalyzer = new ContextAnalyzer();
		}

		#endregion

		#region ReadSource Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadSource_ThrowsIfSourceNull() {
			contextAnalyzer.ReadSource(null);
		}

		[TestMethod()]
		public void ReadSource_SetsSource() {
			IASTNode source = Root();

			contextAnalyzer = contextAnalyzer.ReadSource(source);

			Assert.AreEqual(source, contextAnalyzer.Source);
		}
		#endregion

		#region SetPhases Tests
		[TestMethod()]
		public void SetPhases_SetsPhasesIfGiven() {
			List<Phase> phases = new List<Phase>();

			contextAnalyzer = contextAnalyzer.SetPhases(phases);

			Assert.AreEqual(phases, contextAnalyzer.Phases);
		}

		[TestMethod()]
		public void SetPhases_SetsDefaultPhasesIfNoneGiven() {
			contextAnalyzer = contextAnalyzer.SetPhases(null);

			Assert.IsTrue(contextAnalyzer.Phases.Count == DefaultPhases.DefaultPhasesList.Count);
		}
		#endregion

		#region SetErrorListener Tests
		[TestMethod()]
		public void SetErrorListener_SetErrorListenerIfNull() {
			contextAnalyzer = contextAnalyzer.SetErrorListener(null);

			Assert.IsNotNull(contextAnalyzer.ErrorListener);
		}

		[TestMethod()]
		public void SetErrorListener_SetErrorListenerIfGiven() {
			SemanticErrorListener errorListener = new SemanticErrorListener();

			contextAnalyzer = contextAnalyzer.SetErrorListener(errorListener);

			Assert.AreEqual(errorListener, contextAnalyzer.ErrorListener);
		}
		#endregion

		#region DecorateAST Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecorateAST_ThrowsIfSourceNull() {
			((ContextAnalyzer)contextAnalyzer).Phases = new List<Phase>();
			((ContextAnalyzer)contextAnalyzer).Source = null;
			((ContextAnalyzer)contextAnalyzer).ErrorListener = new SemanticErrorListener();

			contextAnalyzer.DecorateAST();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecorateAST_ThrowsIfPhasesNull() {
			((ContextAnalyzer)contextAnalyzer).Phases = null;
			((ContextAnalyzer)contextAnalyzer).Source = Root();
			((ContextAnalyzer)contextAnalyzer).ErrorListener = new SemanticErrorListener();

			contextAnalyzer.DecorateAST();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecorateAST_ThrowsIfErrorListenerNull() {
			((ContextAnalyzer)contextAnalyzer).Phases = new List<Phase>();
			((ContextAnalyzer)contextAnalyzer).Source = Root();
			((ContextAnalyzer)contextAnalyzer).ErrorListener = null;

			contextAnalyzer.DecorateAST();
		}

		[TestMethod()]
		public void DecorateAST_DecoratesAST() {
			GlobalScopeNode globalScopeNode = Root();

			contextAnalyzer = contextAnalyzer.ReadSource(globalScopeNode).SetErrorListener().SetPhases().DecorateAST();

			Assert.AreEqual(globalScopeNode, contextAnalyzer.DecoratedAST);
		}

		[TestMethod()]
		public void DecorateAST_OutputBecomesNullWithSemanticError() {
			GlobalScopeNode globalScopeNode = Root().With(TypeNode.Void.Function("a")).With(TypeNode.Void.Function("a"));

			contextAnalyzer = contextAnalyzer.ReadSource(globalScopeNode).SetErrorListener().SetPhases().DecorateAST();

			Assert.AreEqual(1, contextAnalyzer.ErrorListener.Errors.Count);
			Assert.IsNull(contextAnalyzer.DecoratedAST);
		}
		#endregion

	}
}
