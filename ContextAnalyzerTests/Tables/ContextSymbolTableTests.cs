using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContextAnalyzer.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodes;
using ContextAnalyzer.SemanticErrors;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;

namespace ContextAnalyzer.Tables.Tests {
	[TestClass()]
	public class ContextSymbolTableTests {

		#region Test Setup

		private ContextSymbolTable contextSymbolTable;

		[TestInitialize()]
		public void Setup() {
			contextSymbolTable = new ContextSymbolTable();
		}

		#endregion

		#region Constructor
		[TestMethod()]
		public void Constructor_EntersScope() {
			Assert.AreEqual(2, contextSymbolTable.ScopeCount);
		}
		#endregion

		#region void Record(ITypedDecl decl)
		[TestMethod()]
		public void Record_CanRecordTest() {
			FuncDeclNode funcDeclNode = TypeNode.Void.Function("testfunc");

			contextSymbolTable.Record(funcDeclNode);

			Assert.AreEqual(funcDeclNode, contextSymbolTable.GetDeclaration("testfunc"));
		}

		[TestMethod()]
		[ExpectedException(typeof(SymbolAlreadyDeclared))]
		public void Record_ThrowsIfSymbolAlreadyDeclaredTest() {
			FuncDeclNode funcDeclNode = TypeNode.Void.Function("testfunc");

			contextSymbolTable.Record(funcDeclNode);
			contextSymbolTable.Record(funcDeclNode);
		}
		#endregion
	}
}
