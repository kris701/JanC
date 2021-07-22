using Nodes;
using CodeGenerator;
using CodeGenerator.ArduinoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CodeGenerator.ArduinoC.Nodes;
using Nodes.ASTHelpers;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using System;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using Tables;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass]
	public class ArduinoSetupTest {
		#region Test Setup

		private ContextSymbolTable contextSymbolTable;
		private PinTable pinTable;
		private ArduinoSetupListener arduinoSetupListener;

		[TestInitialize()]
		public void Setup() {
			pinTable = new PinTable();
			contextSymbolTable = new ContextSymbolTable();
			arduinoSetupListener = new ArduinoSetupListener(contextSymbolTable, pinTable);
		}

		#endregion

		#region Constructor Tests
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfConstructorItemsNull1() {
			new ArduinoSetupListener(null, pinTable);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfConstructorItemsNull2() {
			new ArduinoSetupListener(contextSymbolTable, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfConstructorItemsNull3() {
			new ArduinoSetupListener(null, null);
		}
		#endregion

		#region Enter(GlobalScopeNode node) Tests
		[TestMethod]
		public void AddsSetupAndLoopToBlankScope() {
			GlobalScopeNode globalScopeNode = Root();

			arduinoSetupListener.Enter(globalScopeNode);

			globalScopeNode.Children.First(i => i is FuncDeclNode && ((FuncDeclNode)i).Name == "setup");
			globalScopeNode.Children.First(i => i is FuncDeclNode && ((FuncDeclNode)i).Name == "loop");
		}

		[TestMethod]
		public void AddsLibraryToFreeRTOSInTop() {
			GlobalScopeNode globalScopeNode = Root();

			arduinoSetupListener.Enter(globalScopeNode);

			Assert.IsInstanceOfType(globalScopeNode.Children[0], typeof(LibraryNode));
		}

		[TestMethod]
		[DataRow(5)]
		[DataRow(10)]
		[DataRow(1)]
		public void AddsPinsToSetup(int pinId) {
			pinTable.RecordPinUsage(pinId, Tables.Pins.PinState.OUTPUT, null);
			GlobalScopeNode global = Root()
				.With(BuiltinAST.DigitalWrite(pinId, 1));
			arduinoSetupListener.Enter(global);

			var setup = global.Children.OfType<FuncDeclNode>().First(f => f.Name.Equals("setup"));
			var pinModeNode = ((BlockNode)setup.Body).Content.OfType<PinModeNode>().First();
			Assert.AreEqual(pinId, pinModeNode.PinId);
			Assert.AreEqual(Tables.Pins.PinState.OUTPUT, pinModeNode.State);
		}
		#endregion
	}
}
