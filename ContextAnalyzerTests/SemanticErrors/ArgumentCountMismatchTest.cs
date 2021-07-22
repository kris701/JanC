using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class ArgumentCountMismatchTest {
		#region Constructor
		[TestMethod]
		public void ArgumentCountMismatchCorrectlySetFunctionName() {
			// arrange
			string testString = "test";
			ArgumentCountMismatch argumentCountMismatch;

			// act
			argumentCountMismatch = new ArgumentCountMismatch(testString, 0, 0, null, null);

			// assert
			Assert.AreEqual(testString, argumentCountMismatch.FunctionName);
		}

		[TestMethod]
		public void ArgumentCountMismatchCorrectlySetActualArgs() {
			// arrange
			int testNum = 1;
			ArgumentCountMismatch argumentCountMismatch;

			// act
			argumentCountMismatch = new ArgumentCountMismatch(null, testNum, 0, null, null);

			// assert
			Assert.AreEqual(testNum, argumentCountMismatch.ActualArgs);
		}

		[TestMethod]
		public void ArgumentCountMismatchCorrectlySetExpectedArgs() {
			// arrange
			int testNum = 1;
			ArgumentCountMismatch argumentCountMismatch;

			// act
			argumentCountMismatch = new ArgumentCountMismatch(null, 0, testNum, null, null);

			// assert
			Assert.AreEqual(testNum, argumentCountMismatch.ExpectedArgs);
		}

		[TestMethod]
		public void ArgumentCountMismatchCorrectlySetContext() {
			// arrange
			JanCParser.CallExprContext callExprContext = new JanCParser.CallExprContext(new JanCParser.ExprContext());
			ArgumentCountMismatch argumentCountMismatch;

			// act
			argumentCountMismatch = new ArgumentCountMismatch(null, 0, 0, callExprContext, null);

			// assert
			Assert.AreEqual(callExprContext, argumentCountMismatch.Context);
		}

		[TestMethod]
		public void ArgumentCountMismatchCorrectlySetFuncDeclContext() {
			// arrange
			JanCParser.FuncDeclContext funcDeclContext = new JanCParser.FuncDeclContext(new JanCParser.DeclContext());
			ArgumentCountMismatch argumentCountMismatch;

			// act
			argumentCountMismatch = new ArgumentCountMismatch(null, 0, 0, null, funcDeclContext);

			// assert
			Assert.AreEqual(funcDeclContext, argumentCountMismatch.FuncDeclContext);
		}
		#endregion
		#region GetDescription
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullCallExprContext() {
			// arrange
			ArgumentCountMismatch argumentCountMismatch =
				new ArgumentCountMismatch(
					null,
					0,
					0,
					null,
					new JanCParser.FuncDeclContext(new JanCParser.DeclContext()));

			// act
			argumentCountMismatch.GetDescription();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetDescriptionThrowsOnNullFuncDeclContext() {
			// arrange
			ArgumentCountMismatch argumentCountMismatch =
				new ArgumentCountMismatch(
					null,
					0,
					0,
					new JanCParser.CallExprContext(new JanCParser.ExprContext()),
					null);

			// act
			argumentCountMismatch.GetDescription();
		}

		#endregion
		#region ToString
		[TestMethod]
		public void ToStringReturnsString() {
			// arrange 
			ArgumentCountMismatch argumentCountMismatch =
				new ArgumentCountMismatch(
					null,
					0,
					0,
					new JanCParser.CallExprContext(new JanCParser.ExprContext()),
					new JanCParser.FuncDeclContext(new JanCParser.DeclContext()));

			// act
			string result = argumentCountMismatch.ToString();

			// assert
			Assert.IsNotNull(result);
		}
		#endregion
	}
}
