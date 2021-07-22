using Nodes;
using CodeGenerator.ArduinoC.DomainGenerator;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tools;
using Nodes.ASTHelpers;
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass]
	public class TaskRenamerListenerTests {
		#region Test Setup

		private TaskRenamerListener taskRenamerListener;

		[TestInitialize()]
		public void Setup() {
			taskRenamerListener = new TaskRenamerListener();
		}

		#endregion

		#region Enter(BaseTaskNode node) Tests
		[TestMethod]
		public void RenamesEveryTasks() {
			EveryTaskNode everyTaskNode = EveryTask(100);

			taskRenamerListener.Enter(everyTaskNode);

			Assert.AreEqual("EveryTask0", everyTaskNode.Name);
		}

		[TestMethod]
		public void RenamesOnTasks() {
			OnTaskNode onTaskNode = OnTask(null);

			taskRenamerListener.Enter(onTaskNode);

			Assert.AreEqual("OnTask0", onTaskNode.Name);
		}

		[TestMethod]
		public void RenamesIdleTasks() {
			IdleTaskNode idleTaskNode = IdleTask();

			taskRenamerListener.Enter(idleTaskNode);

			Assert.AreEqual("IdleTask0", idleTaskNode.Name);
		}

		[TestMethod]
		public void RenamesOnceTasks() {
			OnceTaskNode onceTaskNode = OnceTask();

			taskRenamerListener.Enter(onceTaskNode);

			Assert.AreEqual("OnceTask0", onceTaskNode.Name);
		}

		[TestMethod]
		public void RenamesMixedTasks() {
			EveryTaskNode everyTaskNode1 = EveryTask(100);
			OnTaskNode onTaskNode1 = OnTask(null);
			IdleTaskNode idleTaskNode1 = IdleTask();
			OnceTaskNode onceTaskNode1 = OnceTask();

			taskRenamerListener.Enter(everyTaskNode1);
			taskRenamerListener.Enter(onTaskNode1);
			taskRenamerListener.Enter(idleTaskNode1);
			taskRenamerListener.Enter(onceTaskNode1);

			Assert.AreEqual("EveryTask0", everyTaskNode1.Name);
			Assert.AreEqual("OnTask1", onTaskNode1.Name);
			Assert.AreEqual("IdleTask2", idleTaskNode1.Name);
			Assert.AreEqual("OnceTask3", onceTaskNode1.Name);
		}
		#endregion
	}
}
