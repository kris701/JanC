using Nodes.ASTHelpers;
using Nodes;
using CodeGenerator;
using CodeGenerator.ArduinoC;
using CodeGenerator.ArduinoC.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tables.Pins;
using System;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using Tables;
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass]
	public class PinModeVisitorTests {
		#region Test Setup

		private PinTable pinTable;
		private ContextSymbolTable contextSymbolTable;
		private PinCheckerListener pinModeListener;

		private GlobalScopeNode globalScopeNode;

		[TestInitialize()]
		public void Setup() {
			pinTable = new PinTable();
			contextSymbolTable = new ContextSymbolTable();
			pinModeListener = new PinCheckerListener(pinTable, contextSymbolTable);

			globalScopeNode = Root();
		}

		#endregion

		#region Constructor Tests
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfConstructorItemsNull1() {
			new PinCheckerListener(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfConstructorItemsNull2() {
			new PinCheckerListener(pinTable, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfConstructorItemsNull3() {
			new PinCheckerListener(null, contextSymbolTable);
		}
		#endregion

		#region Leave(VarDeclNode node) Tests
		[TestMethod]
		public void RecordsVarDecl() {
			VarDeclNode varDeclNode = Int.VarDecl("test", Literal(5));

			pinModeListener.Leave(varDeclNode);

			Assert.AreEqual(varDeclNode, contextSymbolTable.GetDeclaration("test"));
		}
		#endregion

		#region Leave(CallExprNode node) Tests
		[TestMethod]
		[DataRow(1, false, false)]
		[DataRow(1, true, false)]
		[DataRow(1, false, true)]
		[DataRow(1, true, true)]
		public void RecordsPinState(int pinId, bool isRead, bool isAnalog) {
			CallExprNode callExprNode = getIOMethod(pinId, isRead, isAnalog);

			pinModeListener.Leave(callExprNode);

			Assert.AreEqual(pinId, pinTable.Pins[0].Id);
		}

		[TestMethod]
		[DataRow(5)]
		[DataRow(10)]
		[DataRow(1)]
		public void DoesNotRecordTwice(int pinId) {
			CallExprNode callExprNode1 = getIOMethod(pinId, false, false);
			CallExprNode callExprNode2 = getIOMethod(pinId, false, false);

			pinModeListener.Leave(callExprNode1);
			pinModeListener.Leave(callExprNode2);

			Assert.AreEqual(1, pinTable.Pins.Count);
		}

		[TestMethod]
		[DataRow(5)]
		[DataRow(10)]
		[DataRow(1)]
		public void RecordsStaticPin(int pinId) {
			VarDeclNode identifierExpr = Int.ConstVarDecl("a", Literal(pinId));
			CallExprNode callExprNode = Call(BuiltinAST.DigitalReadDecl);
			callExprNode.Arguments.Add(new ArgNode(null, identifierExpr.Value));

			pinModeListener.Leave(callExprNode);

			Assert.AreEqual(pinId, pinTable.Pins.Where(pin => pin.State == PinState.INPUT).First().Id);
		}
		#endregion

		#region Private Test Methods
		private CallExprNode getIOMethod(int pinId, bool isRead, bool isAnalog) {
			if (isRead) {
				if (isAnalog)
					return BuiltinAST.AnalogRead(pinId);
				else
					return BuiltinAST.DigitalRead(pinId);
			}
			else {
				if (isAnalog)
					return BuiltinAST.AnalogWrite(pinId, 0);
				else
					return BuiltinAST.DigitalWrite(pinId, 0);
			}
		}
		#endregion
	}
}
