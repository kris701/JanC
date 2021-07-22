using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tables.Pins;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class InconsistentPinUsageTest {
		#region Constructor
		[TestMethod]
		public void InconsistentPinUsageCorrectlySetsPinId() {
			// arrange
			int pinId = 1;
			InconsistentPinUsage inconsistentPinUsage;

			// act
			inconsistentPinUsage = new InconsistentPinUsage(pinId, PinState.INPUT, PinState.INPUT, null, null);

			// assert
			Assert.AreEqual(pinId, inconsistentPinUsage.PinId);
		}

		[TestMethod]
		public void InconsistentPinUsageCorrectlySetsNewState() {
			// arrange
			PinState pinState = PinState.OUTPUT;
			InconsistentPinUsage inconsistentPinUsage;

			// act
			inconsistentPinUsage = new InconsistentPinUsage(0, pinState, PinState.INPUT, null, null);

			// assert
			Assert.AreEqual(pinState, inconsistentPinUsage.NewState);
		}

		[TestMethod]
		public void InconsistentPinUsageCorrectlySetsPrevState() {
			// arrange
			PinState pinState = PinState.OUTPUT;
			InconsistentPinUsage inconsistentPinUsage;

			// act
			inconsistentPinUsage = new InconsistentPinUsage(0, PinState.INPUT, pinState, null, null);

			// assert
			Assert.AreEqual(pinState, inconsistentPinUsage.PrevState);
		}

		[TestMethod]
		public void InconsistentPinUsageCorrectlySetsContext() {
			// arrange
			JanCParser.CallExprContext context = new JanCParser.CallExprContext(new JanCParser.ExprContext());
			InconsistentPinUsage inconsistentPinUsage;

			// act
			inconsistentPinUsage = new InconsistentPinUsage(0, PinState.INPUT, PinState.INPUT, context, null);

			// assert
			Assert.AreEqual(context, inconsistentPinUsage.Context);
		}

		[TestMethod]
		public void InconsistentPinUsageCorrectlySetsPrevUsageContext() {
			// arrange
			JanCParser.CallExprContext context = new JanCParser.CallExprContext(new JanCParser.ExprContext());
			InconsistentPinUsage inconsistentPinUsage;

			// act
			inconsistentPinUsage = new InconsistentPinUsage(0, PinState.INPUT, PinState.INPUT, null, context);

			// assert
			Assert.AreEqual(context, inconsistentPinUsage.PrevUsageContext);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullIdentifierContext() {
			// arrange
			InconsistentPinUsage inconsistentPinUsage = new InconsistentPinUsage(
				0,
				PinState.INPUT,
				PinState.INPUT,
				null,
				new JanCParser.CallExprContext(new JanCParser.ExprContext()));

			// act
			inconsistentPinUsage.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullIdentifierPrevUsageContext() {
			// arrange
			InconsistentPinUsage inconsistentPinUsage = new InconsistentPinUsage(
				0,
				PinState.INPUT,
				PinState.INPUT,
				new JanCParser.CallExprContext(new JanCParser.ExprContext()),
				null);

			// act
			inconsistentPinUsage.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			InconsistentPinUsage inconsistentPinUsage = new InconsistentPinUsage(
				0,
				PinState.INPUT,
				PinState.INPUT,
				null,
				null);

			// act
			string result = inconsistentPinUsage.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
