using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tables;
using Tables.Pins;

namespace TableTests {
	[TestClass]
	public class PinTableTests {
		[TestMethod]
		public void CanRecordPinUsage() {
			// ARRANGE
			PinTable pinTable = new PinTable();

			// ACT
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(2, PinState.OUTPUT, null);
			pinTable.RecordPinUsage(3, PinState.INPUT, null);

			// ASSERT
			Assert.AreEqual(3, pinTable.Pins.Count);
			Assert.AreEqual(5, pinTable.Pins[0].Id);
			Assert.AreEqual(PinState.INPUT, pinTable.Pins[0].State);
		}

		[TestMethod]
		public void AllowMultiplePinRecordings() {
			// ARRANGE
			PinTable pinTable = new PinTable();

			// ACT
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.INPUT, null);
			pinTable.RecordPinUsage(5, PinState.INPUT, null);

			// ASSERT
			Assert.AreEqual(1, pinTable.Pins.Count);
			Assert.AreEqual(5, pinTable.Pins[0].Id);
			Assert.AreEqual(PinState.INPUT, pinTable.Pins[0].State);
		}
	}
}
