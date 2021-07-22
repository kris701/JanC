using JanCCLI.CLIParsing;
using JanCCLI.Commands;
using Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace JanCLI.Commands.Tests {
	[TestClass]
	public class PrintSummaryTest {
		[TestMethod]
		public void Summary() {
			// ARRANGE
			Stopwatch stopWatch = new Stopwatch();
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			PrintSummary.Summary(stopWatch, mock.Object);
			string result = output.ToString();

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Summary"));
		}

		[TestMethod]
		public void Summary_WithWarning() {
			// ARRANGE
			Stopwatch stopWatch = new Stopwatch();
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.HadWarnings).Returns(true);
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			PrintSummary.Summary(stopWatch, mock.Object);
			string result = output.ToString();

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Summary"));
		}

		[TestMethod]
		public void Summary_WithError() {
			// ARRANGE
			Stopwatch stopWatch = new Stopwatch();
			Mock<IJanCCompiler> mock = new Mock<IJanCCompiler>();
			mock.Setup(x => x.HadErrors).Returns(true);
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			PrintSummary.Summary(stopWatch, mock.Object);
			string result = output.ToString();

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Summary"));
		}
	}
}
