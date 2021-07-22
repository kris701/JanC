using Antlr4.Runtime.Tree;
using JanCCLI.CLIParsing;
using JanCCLI.Commands;
using Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JanCLI.Commands.Tests {
	[TestClass]
	public class CSTCommandTest {
		#region PrintCST(string, IJanCCompiler, bool, FileInfo)
		[TestMethod]
		public void PrintCST_PrintsAndReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());

			// ACT
			string result = CSTCommand.PrintCST("C:/", mock.Object, false, null);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}

		[TestMethod]
		public void PrintCST_PrintsCSTAndReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());
			mock.Setup(x => x.CSTTree).Returns(new Antlr4.Runtime.ParserRuleContext());

			// ACT
			string result = CSTCommand.PrintCST("C:/", mock.Object, true, null);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}
		#endregion
	}
}
