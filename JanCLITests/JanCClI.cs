using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using JanCCLI;
using Moq;
using JanCCLI.CLIParsing;
using System.IO;

namespace JanCLI.Tests {
	[TestClass]
	public class JanCClITest {
		#region JanCLI
		[TestMethod]
		public void JanCLI_SetsValues() {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI;

			// ACT
			janCCLI = new JanCCLI.JanCCLI();

			// ASSERT
			Assert.IsNotNull(janCCLI._janCCompiler);
			Assert.IsNotNull(janCCLI._argHandler);
		}
		#endregion

		#region ParseCommand
		[TestMethod]
		public void ParseCommand_NoArguments() {
			// ARRANGE
			JanCCLI.JanCCLI jan = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);


			// ACT
			jan.ParseCommand(new string[0]);

			// ASSERT
			Assert.IsTrue(output.ToString().Length > 0);
		}

		[TestMethod]
		public void ParseCommand_OneArgument() {
			// ARRANGE
			JanCCLI.JanCCLI jan = new JanCCLI.JanCCLI();
			jan._argHandler.RegisterCommand("Test", new Command(() => "", new string[1] { "-test" }));

			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			jan.ParseCommand(new string[1] { "-test" });

			// ASSERT
			Assert.IsTrue(output.ToString().Length > 0);
		}

		[TestMethod]
		public void ParseCommand_MultipleArgument() {
			// ARRANGE
			JanCCLI.JanCCLI jan = new JanCCLI.JanCCLI();
			jan._argHandler.RegisterCommand("Test", new Command(() => "", new string[2] { "-test", "-te" }));

			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			jan.ParseCommand(new string[2] { "-test", "-te" });

			// ASSERT
			Assert.IsTrue(output.ToString().Length > 0);
		}
		#endregion
	}
}
