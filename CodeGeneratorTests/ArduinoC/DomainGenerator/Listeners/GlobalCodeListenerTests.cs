using Nodes;
using CodeGenerator.ArduinoC.DomainGenerator;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tools;
using System;
using Nodes.ASTHelpers;
using CodeGenerator.ArduinoC.Nodes;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass]
	public class GlobalCodeListenerTests {
		#region Test Setup

		private ContextSymbolTable contextSymbolTable;
		private GlobalCodeListener globalCodeListener;

		private GlobalScopeNode globalScopeNode;
		private FuncDeclNode setupNode;
		private FuncDeclNode loopNode;

		[TestInitialize()]
		public void Setup() {
			contextSymbolTable = new ContextSymbolTable();
			globalCodeListener = new GlobalCodeListener(contextSymbolTable);

			setupNode = BuiltinAST.SetupDecl;
			loopNode = BuiltinAST.LoopDecl;
			contextSymbolTable.Record(setupNode);
			contextSymbolTable.Record(loopNode);

			globalScopeNode = Root()
				.With(setupNode)
				.With(loopNode);
		}

		#endregion

		#region Constructor Tests
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfConstructorItemNull() {
			new GlobalCodeListener(null);
		}
		#endregion

		#region Enter(GlobalScopeNode global)
		[TestMethod]
		[DataRow(5)]
		[DataRow(10)]
		[DataRow(500)]
		public void MovesAssignmentsToSetupScope(int varValue) {
			VarDeclNode var1 = Int.VarDecl("var1", Literal(varValue));
			globalScopeNode.With(var1);

			globalCodeListener.Enter(globalScopeNode);

			BlockNode setupBlock = (BlockNode)contextSymbolTable.SetupNode.Body;
			var assignStmts = setupBlock.Content.Where(i => i is AssignStmtNode).Select(i => i as AssignStmtNode);
			Assert.AreEqual(varValue, (assignStmts.First(i => ((IdentifierExpr)i.Location).Name == var1.Name).Value as IntLiteralNode).Value);
		}

		[TestMethod]
		[DataRow(5)]
		[DataRow(10)]
		[DataRow(500)]
		public void RemovesAssignmentsFromGlobal(int varValue) {
			VarDeclNode var1 = Int.VarDecl("var1", Literal(varValue));
			globalScopeNode.With(var1);

			globalCodeListener.Enter(globalScopeNode);

			Assert.IsNull(((VarDeclNode)(globalScopeNode.Children.Where(i => i is VarDeclNode).Select(i => i as VarDeclNode).First(i => i.Name == var1.Name))).Value);
		}

		[TestMethod]
		public void MovesDeclarationsUpBeforeSetupScope() {
			VarDeclNode var1 = Int.VarDecl("var1");
			globalScopeNode.With(var1);

			globalCodeListener.Enter(globalScopeNode);

			Assert.IsTrue(globalScopeNode.Content.IndexOf(var1) < globalScopeNode.Content.IndexOf(setupNode));
		}

		[TestMethod]
		public void MovesForwardFunctionDeclareDeclarationsToTop() {
			globalCodeListener.Enter(globalScopeNode);

			Assert.IsInstanceOfType(globalScopeNode.Children[0], typeof(FunctionForwardDeclaration));
			Assert.IsInstanceOfType(globalScopeNode.Children[1], typeof(FunctionForwardDeclaration));
		}

		[TestMethod]
		public void MovesForwardStructDeclareDeclarationsToTop() {
			StructDeclNode structDeclNode = StructDecl("test");
			contextSymbolTable.Record(structDeclNode);
			globalScopeNode.With(structDeclNode);

			globalCodeListener.Enter(globalScopeNode);

			Assert.IsInstanceOfType(globalScopeNode.Children[0], typeof(StructForwardDeclaration));
		}
		#endregion
	}
}
