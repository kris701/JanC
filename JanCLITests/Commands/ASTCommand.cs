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
	public class ASTCommandTest {
		#region PrintAST(string, IJanCCompiler, bool, FileInfo, bool)
		[TestMethod]
		public void PrintAST_PrintsAndReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());

			// ACT
			string result = ASTCommand.PrintAST("C:/", mock.Object, false, null);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}

		[TestMethod]
		public void PrintAST_PrintsASTAndReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());
			mock.Setup(x => x.ASTTree).Returns(new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool));

			// ACT
			string result = ASTCommand.PrintAST("C:/", mock.Object, true, null);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}

		[TestMethod]
		public void PrintAST_PrintsFullASTAndReturnsString() {
			// ARRANGE
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.GenerateCST()).Returns(mock.Object);
			mock.Setup(x => x.GenerateAST()).Returns(mock.Object);
			mock.Setup(x => x.ReadSource(It.IsAny<FileInfo>())).Returns(mock.Object);
			mock.Setup(x => x.HadErrors).Returns(false);
			mock.Setup(x => x.HadWarnings).Returns(false);
			mock.Setup(x => x.GetWarningsSummary());
			mock.Setup(x => x.ASTTree).Returns(new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool));

			// ACT
			string result = ASTCommand.PrintAST("C:/", mock.Object, true, null, true);


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}
		#endregion

		#region PrintAST(IASTNode, int, bool, StringBuilder)
		[TestMethod]
		public void PrintAST_ReturnsString() {
			// ARRANGE


			// ACT
			string result = ASTCommand.PrintAST(new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool));

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}

		[TestMethod]
		public void PrintAST_ReturnsFullNameString() {
			// ARRANGE


			// ACT
			string result = ASTCommand.PrintAST(new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool), 0, true);

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}
		#endregion
	}
}
