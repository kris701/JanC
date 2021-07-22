using JanCCLI.CLIParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JanCLI.CLIParsing.Tests {
	[TestClass]
	public class JanCCli {
		[TestMethod]
		public void GetDescription_ReturnsString() {
			// ARRANGE
			List<Command> commands = new List<Command>();
			commands.Add(new Command(() => "testDelegate", new string[] { "testToken" }));
			NamedCommandCluster testCluster = new NamedCommandCluster("testName", new List<Command>());

			// ACT
			string result = testCluster.GetDescription();

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}
	}
}
