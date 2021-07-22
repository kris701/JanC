using JanCCLI.CLIParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JanCLI.CLIParsing.Tests {
	[TestClass]
	public class ArgHandlerTest {
		#region PrintHelp
		[TestMethod]
		public void PrintHelp() {
			// ARRANGE
			ArgHandler argHandler = new ArgHandler();

			// ACT
			string result = argHandler.PrintHelp();

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
		}
		#endregion

		#region ArgHandler
		[TestMethod]
		public void ArgHandler_AddsHelp() {
			// ARRANGE
			ArgHandler argHandler;

			// ACT
			argHandler = new ArgHandler();

			// ASSERT
			Assert.AreEqual(1, argHandler.CommandList.Count);
			Assert.AreEqual(1, argHandler.CommandList.FindAll(x => x.Name == ArgHandler.printHelpCommandName).Count);
		}
		#endregion

		#region RegisterCommand
		[TestMethod]
		public void RegisterCommand_AddsCommandToCommandList() {
			// ARRANGE
			string name = "test";
			string token = "testToken";
			List<BaseCommand> commands = new List<BaseCommand>();
			Command command = new Command(null, new string[]{token});
			commands.Add(command);
			ArgHandler argHandler = new ArgHandler();

			// ACT
			argHandler.RegisterCommand(name, commands);

			// ASSERT
			Assert.AreEqual(2, argHandler.CommandList.Count);
			Assert.AreEqual(1, argHandler.CommandList.FindAll(x => x.Name == name).Count);
		}

		[TestMethod]
		public void RegisterCommand_AddsSingleCommandToCommandList() {
			// ARRANGE
			string name = "test";
			Command command = new Command(null, new string[] { "" });
			ArgHandler argHandler = new ArgHandler();

			// ACT
			argHandler.RegisterCommand(name, command);

			// ASSERT
			Assert.AreEqual(2, argHandler.CommandList.Count);
			Assert.AreEqual(1, argHandler.CommandList.FindAll(x => x.Name == name).Count);
		}

		[TestMethod]
		public void RegisterCommand_AddsToCommandDictionary() {
			// ARRANGE
			string name = "test";
			string token = "testToken";
			List<BaseCommand> commands = new List<BaseCommand>();
			Command command = new Command(null, new string[] { token });
			commands.Add(command);
			ArgHandler argHandler = new ArgHandler();

			// ACT
			argHandler.RegisterCommand(name, commands);

			// ASSERT
			Assert.AreEqual(2, argHandler.CommandList.Count);
			Assert.AreEqual(1, argHandler.CommandList.FindAll(x => x.Name == name).Count);
			Assert.IsTrue(argHandler.CommandDictionayByToken.ContainsKey(token));
		}

		[TestMethod]
		public void RegisterCommand_AddsMultipleToCommandDictionary() {
			// ARRANGE
			string name1 = "test";
			string token1 = "testToken1";
			string token2 = "testToken2";
			List<BaseCommand> commands = new List<BaseCommand>();
			Command command1 = new Command(null, new string[] { token1 });
			Command command2 = new Command(null, new string[] { token2 });
			commands.Add(command1);
			commands.Add(command2);
			ArgHandler argHandler = new ArgHandler();

			// ACT
			argHandler.RegisterCommand(name1, commands);

			// ASSERT
			Assert.AreEqual(2, argHandler.CommandList.Count);
			Assert.AreEqual(1, argHandler.CommandList.FindAll(x => x.Name == name1).Count);
			Assert.IsTrue(argHandler.CommandDictionayByToken.ContainsKey(token1));
			Assert.IsTrue(argHandler.CommandDictionayByToken.ContainsKey(token2));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterCommand_ExceptionOnRepeatToken() {
			// ARRANGE
			string name1 = "test";
			string token1 = "testToken1";
			List<BaseCommand> commands = new List<BaseCommand>();
			Command command1 = new Command(null, new string[] { token1 });
			Command command2 = new Command(null, new string[] { token1 });
			commands.Add(command1);
			commands.Add(command2);
			ArgHandler argHandler = new ArgHandler();

			// ACT
			argHandler.RegisterCommand(name1, commands);
		}
		#endregion

		#region ParseArgs
		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void ParseArgs_ExceptionOnUnknownArgument() {
			// ARRANGE
			List<string> args = new List<string>() { "test" };
			ArgHandler argHandler = new ArgHandler();

			// ACT
			argHandler.ParseArgs(args).First();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ParseArgs_ExceptionOnNullCommand() {
			// ARRANGE
			string testCommandName = "testCommand";
			string testCommandToken = "-testToken";

			List<string> args = new List<string>() { testCommandToken };
			ArgHandler argHandler = new ArgHandler();
			argHandler.RegisterCommand(testCommandName, new Command(null, new string[] { testCommandToken }));

			// ACT
			argHandler.ParseArgs(args).First();
		}

		[TestMethod]
		public void ParseArgs_CompleteOnCorrect() {
			// ARRANGE
			string testCommandName = "testCommand";
			string testCommandToken = "-testToken";
			string testOutput = "testOutput";

			List<string> args = new List<string>() { testCommandToken };

			ArgHandler argHandler = new ArgHandler();
			argHandler.RegisterCommand(testCommandName, new Command(() =>  testOutput, new string[]{ testCommandToken }));

			// ACT 
			List<string> result = argHandler.ParseArgs(args).ToList();

			// ASSERT
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(testOutput, result[0]);
		}

		[TestMethod]
		public void ParseArgs_CompleteOnMultipleCorrect() {
			// ARRANGE
			string testCommandName1 = "testCommand1";
			string testCommandName2 = "testCommand2";
			string testCommandToken1 = "-testToken1";
			string testCommandToken2 = "-testToken2";
			string testOutput1 = "testOutput1";
			string testOutput2 = "testOutput2";

			List<string> args = new List<string>() { testCommandToken1, testCommandToken2 };

			ArgHandler argHandler = new ArgHandler();
			argHandler.RegisterCommand(testCommandName1, new Command(() => testOutput1, new string[] { testCommandToken1 }));
			argHandler.RegisterCommand(testCommandName2, new Command(() => testOutput2, new string[] { testCommandToken2 }));

			// ACT 
			List<string> result = argHandler.ParseArgs(args).ToList();

			// ASSERT
			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Contains(testOutput1));
			Assert.IsTrue(result.Contains(testOutput2));
		}

		#endregion
	}
}
