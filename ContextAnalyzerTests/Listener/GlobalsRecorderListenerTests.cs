using Nodes;
using ContextAnalyzer;
using ContextAnalyzer.Tables;
using ContextAnalyzer.Listener;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tools;
using System;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.ASTHelpers.CommonAST;

namespace ContextAnalyzer.Listener.Tests {
	[TestClass()]
	public class GlobalsRecorderListenerTests {
		#region Test Setup
		private ContextSymbolTable contextSymbolTable;
		private GlobalsRecorderListener globalsRecorder;

		[TestInitialize]
		public virtual void Setup() {
			contextSymbolTable = new ContextSymbolTable();
			globalsRecorder = new GlobalsRecorderListener(contextSymbolTable);
		}
		#endregion

		#region Constructor Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ThrowsIfInputIsNull() {
			new GlobalsRecorderListener(null);
		}
		#endregion

		#region Enter(ITypeDecl node)
		[TestMethod()]
		public void Enter_RecordsTest() {
			FuncDeclNode funcDeclNode = TypeNode.Void.Function("testfunc");

			globalsRecorder.Enter(funcDeclNode);

			Assert.AreEqual(funcDeclNode, contextSymbolTable.GetDeclaration("testfunc"));
		}
		#endregion
	}
}
