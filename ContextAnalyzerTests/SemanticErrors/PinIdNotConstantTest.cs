using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class PinIdNotConstantTest {
		#region Constructor
		[TestMethod]
		public void PinIdNotConstantCorrectlySetsContext() {
			// arrange
			JanCParser.ArgumentContext context = new JanCParser.ArgumentContext(new Antlr4.Runtime.ParserRuleContext(), 0);
			PinIdNotConstant pinIdNotConstant;

			// act
			pinIdNotConstant = new PinIdNotConstant(context);

			// assert
			Assert.AreEqual(context, pinIdNotConstant.Context);
		}
		#endregion

		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullContext() {
			// arrange
			PinIdNotConstant pinIdNotConstant = new PinIdNotConstant(null);

			// act
			pinIdNotConstant.GetDescription();
		}
		#endregion

		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			PinIdNotConstant pinIdNotConstant = new PinIdNotConstant(null);

			// act
			string result = pinIdNotConstant.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
