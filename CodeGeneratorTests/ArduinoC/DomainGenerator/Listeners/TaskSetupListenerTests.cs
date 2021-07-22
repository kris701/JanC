using Nodes;
using CodeGenerator.ArduinoC.DomainGenerator;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using CodeGenerator.ArduinoC.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tools;
using System;
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass]
	public class TaskSetupListenerTests {
		#region Test Setup

		private ContextSymbolTable symbolTable;
		private TaskSetupListener taskSetupListener;

		private GlobalScopeNode globalScopeNode;
		private FuncDeclNode setupNode;

		[TestInitialize()]
		public void Setup() {
			symbolTable = new ContextSymbolTable();
			taskSetupListener = new TaskSetupListener(symbolTable);

			setupNode = TypeNode.Void.Function("setup");
			symbolTable.Record(setupNode);
			globalScopeNode = Root().With(setupNode);
		}

		#endregion

		#region Constructor Tests
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfConstructorItemsAreNull() {
			new TaskSetupListener(null);
		}
		#endregion

		#region Enter(EveryTaskNode node)
		[TestMethod]
		public void EveryNodes_AddsToSetup() {
			EveryTaskNode everyTaskNode = EveryTask(100);

			taskSetupListener.Enter(everyTaskNode);

			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == everyTaskNode.Name));
		}

		[TestMethod]
		public void EveryNodes_AddsWhileLoop() {
			EveryTaskNode everyTaskNode1 = EveryTask(100);

			taskSetupListener.Enter(everyTaskNode1);

			Assert.IsInstanceOfType(everyTaskNode1.Body.Children[0], typeof(StaticVariableDecl));
			Assert.IsInstanceOfType(everyTaskNode1.Body.Children[1], typeof(WhileStmtNode));
			Assert.AreEqual(1, ((everyTaskNode1.Body.Children[1] as WhileStmtNode).Condition as IntLiteralNode).Value);
		}
		[TestMethod]
		public void EveryNodes_AddsConcurrentSleep() {
			EveryTaskNode everyTaskNode1 = EveryTask(100);

			taskSetupListener.Enter(everyTaskNode1);

			Assert.IsInstanceOfType((everyTaskNode1.Body.Children[1] as WhileStmtNode).Body.Children[0], typeof(LongVariableDecl));
			Assert.IsInstanceOfType((everyTaskNode1.Body.Children[1] as WhileStmtNode).Body.Children[1], typeof(IfStmtNode));
		}
		#endregion

		#region Enter(OnTaskNode node)
		[TestMethod]
		public void OnNodes_AddsToSetup() {
			OnTaskNode onTaskNode = OnTask(null);

			taskSetupListener.Enter(onTaskNode);

			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == onTaskNode.Name));
		}

		[TestMethod]
		public void OnTaskNodes_AddsWhileLoop() {
			OnTaskNode onTaskNode1 = OnTask(new BinaryExprNode(null, new IdentifierExpr(null, "a"), "==", new IntLiteralNode(null, 5)));

			taskSetupListener.Enter(onTaskNode1);

			Assert.IsInstanceOfType(((WhileStmtNode)onTaskNode1.Body.Children[0]).Body.Children[0], typeof(WhileStmtNode));
			Assert.IsInstanceOfType((((WhileStmtNode)onTaskNode1.Body.Children[0]).Body.Children[0] as WhileStmtNode).Condition, typeof(NotNode));
			Assert.AreEqual("a", (((((((WhileStmtNode)onTaskNode1.Body.Children[0]).Body.Children[0] as WhileStmtNode).Condition as NotNode).Value as GroupingExprNode).Value as BinaryExprNode).Left as IdentifierExpr).Name);
			Assert.AreEqual(5, (((((((WhileStmtNode)onTaskNode1.Body.Children[0]).Body.Children[0] as WhileStmtNode).Condition as NotNode).Value as GroupingExprNode).Value as BinaryExprNode).Right as IntLiteralNode).Value);
		}
		#endregion

		#region Enter(OnceTaskNode node)
		[TestMethod]
		public void OnceNodes_AddsToSetup() {
			OnceTaskNode onceTaskNode = OnceTask();

			taskSetupListener.Enter(onceTaskNode);

			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == onceTaskNode.Name));
		}
		#endregion

		#region Enter(IdleTaskNode node)
		[TestMethod]
		public void IdleNodes_AddsToSetup() {
			IdleTaskNode idleTaskNode = IdleTask();

			taskSetupListener.Enter(idleTaskNode);

			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == idleTaskNode.Name));
		}

		[TestMethod]
		public void IdleTaskNodes_AddsWhileLoop() {
			IdleTaskNode idleTaskNode = IdleTask();

			taskSetupListener.Enter(idleTaskNode);

			Assert.IsInstanceOfType(idleTaskNode.Body.Children[0], typeof(WhileStmtNode));
			Assert.AreEqual(1, ((idleTaskNode.Body.Children[0] as WhileStmtNode).Condition as IntLiteralNode).Value);
		}

		[TestMethod]
		public void IdleTaskNodes_AddsConcurrentSleep() {
			IdleTaskNode idleTaskNode = IdleTask();

			taskSetupListener.Enter(idleTaskNode);

			Assert.IsNotNull((idleTaskNode.Body.Children[0] as WhileStmtNode).Body.Children.FindLast(node => node is ConcurrentSleepNode));
			Assert.AreEqual("100", ((idleTaskNode.Body.Children[0] as WhileStmtNode).Body.Children.FindLast(node => node is ConcurrentSleepNode) as ConcurrentSleepNode).Delay);
		}
		#endregion

		[TestMethod]
		public void AddsMixesNodesToSetup() {
			EveryTaskNode everyTaskNode1 = EveryTask(100);
			OnTaskNode onTaskNode1 = OnTask(null);
			OnceTaskNode onceTaskNode1 = OnceTask();
			IdleTaskNode idleTaskNode1 = IdleTask();

			taskSetupListener.Enter(everyTaskNode1);
			taskSetupListener.Enter(onTaskNode1);
			taskSetupListener.Enter(onceTaskNode1);
			taskSetupListener.Enter(idleTaskNode1);

			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == everyTaskNode1.Name));
			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == onTaskNode1.Name));
			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == onceTaskNode1.Name));
			Assert.IsNotNull(symbolTable.SetupNode.Body.Children.Find(node => node is TaskSetupNode && (node as TaskSetupNode).Name == idleTaskNode1.Name));
		}
	}
}
