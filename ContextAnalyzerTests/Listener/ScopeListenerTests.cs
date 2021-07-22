using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContextAnalyzer.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContextAnalyzer.Tables;
using Tables;
using Nodes;
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;

namespace ContextAnalyzer.Listener.Tests {
	[TestClass()]
	public class ScopeListenerTests {
		#region Test Setup
		private ContextSymbolTable contextSymbolTable;
		private ScopeListener scopeListener;

		[TestInitialize]
		public virtual void Setup() {
			contextSymbolTable = new ContextSymbolTable();
			scopeListener = new ScopeListener(contextSymbolTable);
		}
		#endregion

		#region Constructor Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_ThrowIfConstructorItemsNull1() {
			new ScopeListener(null);
		}
		#endregion

		#region FuncDeclNode
		[TestMethod()]
		public void Enter_FuncDeclNode_EntersScope() {
			FuncDeclNode funcDeclNode = Int.Function("func");

			scopeListener.Enter(funcDeclNode);

			Assert.AreEqual(3, contextSymbolTable.ScopeCount);
		}
		[TestMethod()]
		public void Leave_FuncDeclNode_LeavesScope() {
			FuncDeclNode funcDeclNode = Int.Function("func");

			scopeListener.Leave(funcDeclNode);

			Assert.AreEqual(1, contextSymbolTable.ScopeCount);
		}
		#endregion

		#region ForStmtNode
		[TestMethod()]
		public void Enter_ForStmtNode_EntersScope() {
			ForStmtNode forStmtNode = new ForStmtNode(null, null, null, null, null);

			scopeListener.Enter(forStmtNode);

			Assert.AreEqual(3, contextSymbolTable.ScopeCount);
		}
		[TestMethod()]
		public void Leave_ForStmtNode_LeavesScope() {
			ForStmtNode forStmtNode = new ForStmtNode(null, null, null, null, null);

			scopeListener.Leave(forStmtNode);

			Assert.AreEqual(1, contextSymbolTable.ScopeCount);
		}
		#endregion

		#region BlockNode
		[TestMethod()]
		public void Enter_BlockNode_EntersScope() {
			BlockNode blockNode = new BlockNode(null, null);

			scopeListener.Enter(blockNode);

			Assert.AreEqual(3, contextSymbolTable.ScopeCount);
		}
		[TestMethod()]
		public void Leave_BlockNode_LeavesScope() {
			BlockNode blockNode = new BlockNode(null, null);

			scopeListener.Leave(blockNode);

			Assert.AreEqual(1, contextSymbolTable.ScopeCount);
		}
		#endregion

		#region StructDeclNode
		[TestMethod()]
		public void Enter_StructDeclNode_EntersScope() {
			StructDeclNode structDeclNode = new StructDeclNode(null, null, null);

			scopeListener.Enter(structDeclNode);

			Assert.AreEqual(3, contextSymbolTable.ScopeCount);
		}
		[TestMethod()]
		public void Leave_StructDeclNode_LeavesScope() {
			StructDeclNode structDeclNode = new StructDeclNode(null, null, null);

			scopeListener.Leave(structDeclNode);

			Assert.AreEqual(1, contextSymbolTable.ScopeCount);
		}
		#endregion

		#region ModuleDeclNode
		[TestMethod()]
		public void Enter_ModuleDeclNode_EntersScope() {
			ModuleDeclNode moduleDeclNode = Module("mod");

			scopeListener.Enter(moduleDeclNode);

			Assert.AreEqual(3, contextSymbolTable.ScopeCount);
		}
		[TestMethod()]
		public void Leave_ModuleDeclNode_LeavesScope() {
			ModuleDeclNode moduleDeclNode = Module("mod");

			scopeListener.Leave(moduleDeclNode);

			Assert.AreEqual(1, contextSymbolTable.ScopeCount);
		}
		[TestMethod()]
		public void Leave_ModuleDeclNode_RecordsInnerContent() {
			ModuleDeclNode innerModuleDeclNode = Module("a");
			ModuleDeclNode moduleDeclNode = Module("mod").With(innerModuleDeclNode);

			scopeListener.Enter(moduleDeclNode);

			Assert.AreEqual(contextSymbolTable.GetDeclaration("a"), innerModuleDeclNode);
		}
		#endregion

		#region VarDeclNode
		[TestMethod()]
		public void Leave_VarDeclNode_Records() {
			VarDeclNode varDeclNode = Int.VarDecl("var");

			scopeListener.Leave(varDeclNode);

			Assert.AreEqual(contextSymbolTable.GetDeclaration("var"), varDeclNode);
		}
		#endregion
	}
}
