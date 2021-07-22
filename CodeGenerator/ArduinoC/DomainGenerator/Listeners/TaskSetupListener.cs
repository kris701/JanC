using Nodes;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using CodeGenerator.ArduinoC.Nodes;
using System.Collections.Generic;
using Tools;
using Nodes.ASTHelpers;
using System;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;
using static Nodes.TypeNode;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	internal class TaskSetupListener : ASTListener {
		private readonly ContextSymbolTable _symbolTable;

		public TaskSetupListener(ContextSymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		#region Every Task Nodes

		public void Enter(EveryTaskNode node) {
			WrapInIfStatement(node, Int32.Parse(node.Delay));
			AddCurrentMillisVar(node);
			AddConcurrentSleep(node, "50");
			WrapInInfiniteLoop(node);
			AddPreviousMillisVar(node);
			AddTaskSetupToSetup(node);
		}

		private static void AddPreviousMillisVar(BaseTaskNode node) {
			node.PrependBody(new StaticVariableDecl(new LongVariableDecl($"{node.Name}_previousMillis", Literal(0))));
		}

		private static void AddCurrentMillisVar(BaseTaskNode node) {
			node.PrependBody(new LongVariableDecl($"{node.Name}_currentMillis", UndecoratedAST.Call("millis")));
		}

		private static void WrapInIfStatement(BaseTaskNode node, int interval) {
			node.PrependBody(CommonAST.Assign(UndecoratedAST.Identifier($"{node.Name}_previousMillis"), UndecoratedAST.Identifier($"{node.Name}_currentMillis")));
			node.Body = new BlockNode(
				content: new List<IUnit>() {
						new IfStmtNode(
							condition: new BinaryExprNode( null,
								new BinaryExprNode( null,
									UndecoratedAST.Identifier($"{node.Name}_currentMillis"),
									"-",
									UndecoratedAST.Identifier($"{node.Name}_previousMillis")),
								">",
								Literal(interval)),
							thenBody: node.Body,
							context: null,
							elseBody: null
						)
				},
				context: null
			);
		}

		#endregion

		#region On Task Nodes

		public void Enter(OnTaskNode node) {
			ConstructOnTaskNode(node);
			AddTaskSetupToSetup(node);
			WrapInInfiniteLoop(node);
		}

		private static void ConstructOnTaskNode(OnTaskNode node) {
			node.PrependBody(new WhileStmtNode(
				condition: new NotNode(value: new GroupingExprNode(value: node.Condition, context: null), context: null),
				body: new ConcurrentSleepNode("100"),
				context: null
			));
			node.AppendBody(new WhileStmtNode(
				condition: new GroupingExprNode(value: node.Condition, context: null),
				body: new ConcurrentSleepNode("100"),
				context: null
			));
		}

		#endregion

		#region Once Task Nodes

		public void Enter(OnceTaskNode node) {
			AddTaskSetupToSetup(node);
		}

		#endregion

		#region Idle Task Nodes

		public void Enter(IdleTaskNode node) {
			AddConcurrentSleep(node, "100");
			WrapInInfiniteLoop(node);
			AddTaskSetupToSetup(node);
		}

		#endregion

		private void AddTaskSetupToSetup(BaseTaskNode node) {
			_symbolTable.SetupNode.AppendBody(new TaskSetupNode(node.Name));
		}

		private static void WrapInInfiniteLoop(BaseTaskNode node) {
			node.Body = new BlockNode(
				content: new List<IUnit>() {
						new WhileStmtNode(
							condition: new IntLiteralNode(value: 1, context: null),
							body: node.Body,
							context: null
						)
				},
				context: null
			);
		}

		private static void AddConcurrentSleep(BaseTaskNode node, string delay) {
			node.AppendBody(new ConcurrentSleepNode(delay: delay));
		}
	}
}
