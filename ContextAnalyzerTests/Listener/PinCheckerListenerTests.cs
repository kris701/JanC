using ContextAnalyzer.SemanticErrors;
using ContextAnalyzer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using Tables.Pins;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.ASTHelpers.CommonAST;
using Nodes.ASTHelpers;

namespace ContextAnalyzer.Listener.Tests {
	[TestClass()]
	public class PinCheckerListenerTests {
		#region Test Setup
		private ContextPinTable contextPinTable;
		private ContextSymbolTable contextSymbolTable;
		private PinCheckerListener pinCheckerListener;

		[TestInitialize]
		public virtual void Setup() {
			contextPinTable = new ContextPinTable();
			contextSymbolTable = new ContextSymbolTable();
			pinCheckerListener = new PinCheckerListener(contextPinTable, contextSymbolTable);
		}
		#endregion

		#region Constructor Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ThrowsIfConstructorItemsNull1() {
			new PinCheckerListener(null, contextSymbolTable);
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ThrowsIfConstructorItemsNull2() {
			new PinCheckerListener(contextPinTable, null);
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ThrowsIfConstructorItemsNull3() {
			new PinCheckerListener(null, null);
		}
		#endregion

		#region void Leave(CallExprNode node) Tests
		[TestMethod()]
		[DataRow("digitalWrite", PinState.OUTPUT)]
		[DataRow("analogWrite", PinState.OUTPUT)]
		[DataRow("digitalRead", PinState.INPUT)]
		[DataRow("analogRead", PinState.INPUT)]
		public void Leave_CanRecordAllIOTypesTest(string funcName, PinState expectedState) {
			CallExprNode callExprNode = Call(funcName).With(Argument(1));
			callExprNode.Item.Type = new UserDefinedTypeNode(null, funcName, UserDefinedTypeNode.Types.Function);

			pinCheckerListener.Leave(callExprNode);

			Assert.AreEqual(expectedState, contextPinTable.Pins[0].State);
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentCountMismatchBuildInFunction))]
		public void Leave_ThrowIfNoArgumentsGiven() {
			CallExprNode callExprNode = Call("digitalRead");
			callExprNode.Item.Type = BuiltinAST.DigitalReadDecl.Type;

			pinCheckerListener.Leave(callExprNode);
		}
		[ExpectedException(typeof(ArgumentCountMismatchBuildInFunction))]
		public void Leave_ThrowIfPinIDNotConstantTest() {
			CallExprNode callExprNode = Call("digitalRead").With(Argument(Identifier("a")));
			callExprNode.Item.Type = BuiltinAST.DigitalReadDecl.Type;

			pinCheckerListener.Leave(callExprNode);
		}
		#endregion
	}
}
