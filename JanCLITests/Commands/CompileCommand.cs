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
	public class CompileCommandTest {
		#region PrintCompile
		[TestMethod]
		public void PrintCompile_ReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.DecorateAST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateCode()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());

			// ACT
			string result = CompileCommand.PrintCompile("C:/", mock.Object, false, null);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}

		[TestMethod]
		public void PrintCompile_PrintsAndReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.DecorateAST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateCode()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());

			// ACT
			string result = CompileCommand.PrintCompile("C:/", mock.Object, true, null);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}
		#endregion
	}
}
