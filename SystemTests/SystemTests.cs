using JanCCLI;
using Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SystemTests {
	[TestClass]
    public class SystemTests
    {
		private string inputPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Input/";
		private string outputPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/ExpectedOutput/";

		[TestMethod]
		[DataRow("BlankScopeTest1.jc", "BlankScopeTest1_output.txt")]
		public void BaseTests(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[TestCategory("slow test")]
		[DataRow("TaskTest1.jc", "TaskTest1_output.txt")]
		[DataRow("TaskTest2.jc", "TaskTest2_output.txt")]
		public void Tasks(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[DataRow("FunctionsTest1.jc", "FunctionsTest1_output.txt")]
		[DataRow("FunctionsTest2.jc", "FunctionsTest2_output.txt")]
		[DataRow("BuildInFunctionsTest1.jc", "BuildInFunctionsTest1_output.txt")]
		[DataRow("BuildInFunctionsTest2.jc", "BuildInFunctionsTest2_output.txt")]
		public void Functions(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[DataRow("AssignmentTest1.jc", "AssignmentTest1_output.txt")]
		[DataRow("AssignmentTest2.jc", "AssignmentTest2_output.txt")]
		[DataRow("AssignmentTest3.jc", "AssignmentTest3_output.txt")]
		[DataRow("AssignmentTest4.jc", "AssignmentTest4_output.txt")]
		public void Assignment(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[DataRow("LoopsTest1.jc", "LoopsTest1_output.txt")]
		[DataRow("LoopsTest2.jc", "LoopsTest2_output.txt")]
		public void Loops(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[DataRow("ConditionalsTest1.jc", "ConditionalsTest1_output.txt")]
		[DataRow("ConditionalsTest2.jc", "ConditionalsTest2_output.txt")]
		public void Conditionals(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[DataRow("StructTest1.jc", "StructTest1_output.txt")]
		[DataRow("StructTest2.jc", "StructTest2_output.txt")]
		[DataRow("StructTest3.jc", "StructTest3_output.txt")]
		public void Structs(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		[DataRow("RefAssignmentAndBinaryExprTest.jc", "RefAssignmentAndBinaryExprTest_output.txt")]
		public void Refs(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		[TestMethod]
		//[DataRow("ModuleTest1.jc", "ModuleTest1_output.txt")]
		//[DataRow("ModuleTest2.jc", "ModuleTest2_output.txt")]
		//[DataRow("ModuleTest3.jc", "ModuleTest3_output.txt")]
		[DataRow("ModuleTest4.jc", "ModuleTest4_output.txt")]
		public void Modules(string inputFile, string outputFile) {
			IsOutputCorrect(inputFile, outputFile);
		}

		#region JanCLITest
		#region Main
		[TestMethod]
		[DataRow("HelloWorldTest.jc")]
		public void Main_CanCompile(string inputFile) {
			// ARRANGE
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			JanCCLI.Program.Main(new string[3] { "-print", "-c", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Compile Started"));
			Assert.IsTrue(result.Contains("Output Code"));
			Assert.IsTrue(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}
		#endregion
		#region Commands
		#region Compile Command
		[TestMethod]
		[DataRow("HelloWorldTest.jc")]
		// The following are random tests from other system tests
		// They do however, serve the purpose of testing the CLI
		[DataRow("AssignmentTest1.jc")]
		[DataRow("BlankScopeTest1.jc")]
		[DataRow("BuildInFunctionsTest1.jc")]
		[DataRow("ConditionalsTest1.jc")]
		public void JanCLI_CompilePrintsToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-c", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Compile Started"));
			Assert.IsTrue(result.Contains("Output Code"));
			Assert.IsTrue(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("ErrorTest.jc")]
		
		public void JanCLI_CompileCatchesErrorToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-c", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Compile Started"));
			Assert.IsFalse(result.Contains("Output Code"));
			Assert.IsFalse(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("HelloWorldTest.jc", "HelloWorldTest_output.txt")]
		public void JanCLI_CompilePrintsToFile(string inputFile, string outputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			string oFile = outputFile;

			// ACT
			janCCLI.ParseCommand(new string[4] { "-o", oFile, "-c", inputPath + inputFile });
			string result = File.ReadAllText(oFile);
			File.Delete(oFile);

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("#include <Arduino_FreeRTOS.h>"));
		}
		#endregion
		#region CSTCommand
		[TestMethod]
		[DataRow("HelloWorldTest.jc")]
		// The following are random tests from other system tests
		// They do however, serve the purpose of testing the CLI
		[DataRow("AssignmentTest1.jc")]
		[DataRow("BlankScopeTest1.jc")]
		[DataRow("BuildInFunctionsTest1.jc")]
		[DataRow("ConditionalsTest1.jc")]
		public void JanCLI_CSTPrintsToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-cst", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Generating CST"));
			Assert.IsTrue(result.Contains("Summary"));
			Assert.IsTrue(result.Contains("CST Tree"));
			Assert.IsFalse(result.Contains("CST Tree written to file"));
			Assert.IsTrue(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("ErrorTest.jc")]

		public void JanCLI_CSTCatchesErrorToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-cst", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Generating CST"));
			Assert.IsTrue(result.Contains("Summary"));
			Assert.IsFalse(result.Contains("CST Tree"));
			Assert.IsFalse(result.Contains("CST Tree written to file"));
			Assert.IsFalse(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("HelloWorldTest.jc", "HelloWorldTest_output.txt")]
		public void JanCLI_CSTPrintsToFile(string inputFile, string outputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			string oFile = outputFile;

			// ACT
			janCCLI.ParseCommand(new string[4] { "-o", oFile, "-cst", inputPath + inputFile });
			string result = File.ReadAllText(oFile);
			File.Delete(oFile);

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("<JanCParser+CompileUnitContext>"));
		}
		#endregion
		#region ASTCommand
		[TestMethod]
		[DataRow("HelloWorldTest.jc")]
		// The following are random tests from other system tests
		// They do however, serve the purpose of testing the CLI
		[DataRow("AssignmentTest1.jc")]
		[DataRow("BlankScopeTest1.jc")]
		[DataRow("BuildInFunctionsTest1.jc")]
		[DataRow("ConditionalsTest1.jc")]
		public void JanCLI_ASTPrintsToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-ast", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Generating AST"));
			Assert.IsTrue(result.Contains("Summary"));
			Assert.IsTrue(result.Contains("AST Tree"));
			Assert.IsFalse(result.Contains("AST Tree written to file"));
			Assert.IsTrue(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("HelloWorldTest.jc")]
		// The following are random tests from other system tests
		// They do however, serve the purpose of testing the CLI
		[DataRow("AssignmentTest1.jc")]
		[DataRow("BlankScopeTest1.jc")]
		[DataRow("BuildInFunctionsTest1.jc")]
		[DataRow("ConditionalsTest1.jc")]
		public void JanCLI_FullASTPrintsToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-ast-full", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Generating Full AST"));
			Assert.IsTrue(result.Contains("Summary"));
			Assert.IsTrue(result.Contains("AST Tree"));
			Assert.IsFalse(result.Contains("AST Tree written to file"));
			Assert.IsTrue(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("ErrorTest.jc")]

		public void JanCLI_ASTCatchesErrorToConsole(string inputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			var output = new StringWriter();
			Console.SetOut(output);

			// ACT
			janCCLI.ParseCommand(new string[3] { "-print", "-ast", inputPath + inputFile });
			string result = output.ToString();


			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("Generating AST"));
			Assert.IsTrue(result.Contains("Summary"));
			Assert.IsFalse(result.Contains("AST Tree"));
			Assert.IsFalse(result.Contains("AST Tree written to file"));
			Assert.IsFalse(result.Contains("No errors"));
			Assert.IsTrue(result.Contains("No warnings"));
		}

		[TestMethod]
		[DataRow("HelloWorldTest.jc", "HelloWorldTest_output.txt")]
		public void JanCLI_ASTPrintsToFile(string inputFile, string outputFile) {
			// ARRANGE
			JanCCLI.JanCCLI janCCLI = new JanCCLI.JanCCLI();
			string oFile = outputFile;

			// ACT
			janCCLI.ParseCommand(new string[4] { "-o", oFile, "-ast", inputPath + inputFile });
			string result = File.ReadAllText(oFile);
			File.Delete(oFile);

			// ASSERT
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length > 0);
			Assert.IsTrue(result.Contains("<GlobalScopeNode>"));
		}
		#endregion
		#endregion
		#endregion

		#region Test methods

		private void IsOutputCorrect(string inputFile, string outputFile) {
			// ARRANGE
			IJanCCompiler janCCompiler = new JanCCompiler();

			// ACT
			janCCompiler = janCCompiler.ReadSource(new FileInfo(inputPath + inputFile)).GenerateCST().GenerateAST().DecorateAST().GenerateCode();

			// ASSERT
			Assert.IsTrue(CheckIfIsSame(janCCompiler.CompiledCCode, File.ReadAllText(outputPath + outputFile)));
		}

		private bool CheckIfIsSame(string input, string output) {
			Regex lineSplit = new Regex(@"\r?\n");

			string[] inputSplit = lineSplit.Split(input);
			string[] outputSplit = lineSplit.Split(output);

			StringBuilder sb = new StringBuilder();

			if (inputSplit.Length != outputSplit.Length) {
				sb.AppendLine($"Error, input and output code not same line length! (input: {inputSplit.Length + 1}, output: {outputSplit.Length + 1})");
				sb.AppendLine();
			}

			for(int i = 0; i < outputSplit.Length; i++) {
				if (i >= inputSplit.Length) {
					sb.AppendLine($"Error, input and output not the same at line {i + 1}!");
					sb.AppendLine($"Expected:");
					sb.AppendLine(outputSplit[i]);
					sb.AppendLine($"Actual:");
					sb.AppendLine("< EMPTY >");
					sb.AppendLine();
					break;
				}
				if (inputSplit[i] != outputSplit[i]) {
					sb.AppendLine($"Error, input and output not the same at line {i + 1}!");
					sb.AppendLine($"Expected:");
					sb.AppendLine(outputSplit[i]);
					sb.AppendLine($"Actual:");
					sb.AppendLine(inputSplit[i]);
					sb.AppendLine(GetIndexCursor(inputSplit, outputSplit, i));
					sb.AppendLine();
				}
			}

			if (sb.Length != 0)
				Assert.Fail(sb.ToString());

			return true;
		}

		private string GetIndexCursor(string[] inputSplit, string[] outputSplit, int index) {
			string spaces = "";
			int highIndex = outputSplit[index].Length;
			if (inputSplit[index].Length > highIndex)
				highIndex = inputSplit[index].Length;

			for (int j = 0; j < highIndex; j++) {
				if (j >= inputSplit[index].Length) {
					spaces += "^";
					break;
				}
				if (j >= outputSplit[index].Length) {
					spaces += "^";
					break;
				}
				if (inputSplit[index][j] == outputSplit[index][j]) {
					if (inputSplit[index][j] == '\t')
						spaces += "\t";
					else
						spaces += " ";
				}
				else {
					spaces += "^";
					break;
				}
			}
			return spaces;
		}

		#endregion
	}
}
