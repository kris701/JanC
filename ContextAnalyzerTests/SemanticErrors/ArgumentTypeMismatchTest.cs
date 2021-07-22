using Nodes;
using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContextAnalyzer.SemanticErrors {
	[TestClass]
	public class ArgumentTypeMismatchTest {
		#region Constructor
		[TestMethod]
		public void ArgumentTypeMismatchCorrectlySetsCallExpr() {
			// arrange
			CallExprNode testCallExprNode = new CallExprNode(null, null, null);
			ArgumentTypeMismatch argumentTypeMismatch;

			// act
			argumentTypeMismatch = new ArgumentTypeMismatch(testCallExprNode, 0, null, null, null);

			// assert
			Assert.AreEqual(testCallExprNode, argumentTypeMismatch.CallExpr);
		}

		[TestMethod]
		public void ArgumentTypeMismatchCorrectlySetsArgumentIndex() {
			// arrange
			int testArgumentIndex = 1;
			ArgumentTypeMismatch argumentTypeMismatch;

			// act
			argumentTypeMismatch = new ArgumentTypeMismatch(null, 1, null, null, null);

			// assert
			Assert.AreEqual(testArgumentIndex, argumentTypeMismatch.ArgumentIndex);
		}

		[TestMethod]
		public void ArgumentTypeMismatchCorrectlySetsArgNode() {
			// arrange
			ArgNode testArgNode = new ArgNode(null, null);
			ArgumentTypeMismatch argumentTypeMismatch;

			// act
			argumentTypeMismatch = new ArgumentTypeMismatch(null, 0, testArgNode, null, null);

			// assert
			Assert.AreEqual(testArgNode, argumentTypeMismatch.Argument);
		}

		[TestMethod]
		public void ArgumentTypeMismatchCorrectlySetsParameter() {
			// arrange
			VarDeclNode testParamNode = new VarDeclNode(null, null, null, null, false);
			ArgumentTypeMismatch argumentTypeMismatch;

			// act
			argumentTypeMismatch = new ArgumentTypeMismatch(null, 0, null, testParamNode, null);

			// assert
			Assert.AreEqual(testParamNode, argumentTypeMismatch.Parameter);
		}

		[TestMethod]
		public void ArgumentTypeMismatchCorrectlySetsFuncDecl() {
			// arrange
			FuncDeclNode testFuncDeclNode = new FuncDeclNode(null, null, null, null, null);
			ArgumentTypeMismatch argumentTypeMismatch;

			// act
			argumentTypeMismatch = new ArgumentTypeMismatch(null, 0, null, null, testFuncDeclNode);

			// assert
			Assert.AreEqual(testFuncDeclNode, argumentTypeMismatch.FuncDecl);
		}
		#endregion
	}
}
