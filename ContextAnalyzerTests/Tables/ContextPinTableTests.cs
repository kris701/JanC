using ContextAnalyzer.SemanticErrors;
using ContextAnalyzer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tables.Pins;

namespace ContextAnalyzer.Tables.Tests {
	[TestClass]
	public class ContextPinTableTests {
		#region Test Setup

		private ContextPinTable pinTable;

		[TestInitialize()]
		public void Setup() {
			pinTable = new ContextPinTable();
		}


		#endregion

		#region Constructor
		[TestMethod]
		public void Constructor_CreatesDictionaryTest() {
			Assert.IsNotNull(pinTable.Pins);
		}
		#endregion

		#region RecordPinUsage(int pinId, PinState usage, JanCParser.CallExprContext context)

		[TestMethod]
		public void RecordPinUsage_CanRecordPinTest() {
			pinTable.RecordPinUsage(5, PinState.INPUT, null);

			Assert.AreEqual(5, pinTable.Pins[0].Id);
			Assert.AreEqual(PinState.INPUT, pinTable.Pins[0].State);
		}

		[TestMethod]
		public void RecordPinUsage_WontRecordMultipleOfSamePin() {
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.INPUT, null);

			Assert.AreEqual(1, pinTable.Pins.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InconsistentPinUsage))]
		public void RecordPinUsage_ThrowIfRecordingAsBothInAndOutputTest() {
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.OUTPUT, null);
		}

		#endregion
	}
}
