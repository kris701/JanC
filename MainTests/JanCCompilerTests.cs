using CodeGenerator;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using Tools;
using ContextAnalyzer;
using CSTGenerator;
using Antlr4.Runtime;
using ContextAnalyzer.SemanticErrors;
using ASTGenerator;
using Exceptions.Exceptions.Base;
using CodeGenerator.ArduinoC;
using Compiler.Exceptions;
using Nodes.ASTHelpers;

namespace Compiler.Tests {
	[TestClass()]
	public class JanCCompilerTests {
		#region Test Setup
		private string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/TestFiles/";

		private IJanCCompiler janCCompiler;

		[TestInitialize()]
		public void Setup() {
			janCCompiler = new JanCCompiler();
		}

		#endregion

		#region Constructor Tests

		[TestMethod()]
		public void JanCCompiler_ConstructorTest1() {
			Assert.IsNotNull(janCCompiler.CSTGenerator);
			Assert.IsNotNull(janCCompiler.ASTGenerator);
			Assert.IsNotNull(janCCompiler.ContextAnalyzer);
			Assert.IsNotNull(janCCompiler.CodeGenerator);
		}
		[TestMethod()]
		public void JanCCompiler_ConstructorTest2() {
			ICodeGenerator codeGenerator = new ArduinoCGenerator();

			janCCompiler = new JanCCompiler(codeGenerator: codeGenerator);

			Assert.AreEqual(codeGenerator, janCCompiler.CodeGenerator);
		}
		[TestMethod()]
		public void JanCCompiler_ConstructorTest3() {
			IASTBuilder aSTBuilder = new ASTBuilder();

			janCCompiler = new JanCCompiler(ASTGenerator: aSTBuilder);

			Assert.AreEqual(aSTBuilder, janCCompiler.ASTGenerator);
		}
		[TestMethod()]
		public void JanCCompiler_ConstructorTest4() {
			ICSTBuilder cSTBuilder = new CSTBuilder();

			janCCompiler = new JanCCompiler(CSTGenerator: cSTBuilder);

			Assert.AreEqual(cSTBuilder, janCCompiler.CSTGenerator);
		}
		[TestMethod()]
		public void JanCCompiler_ConstructorTest5() {
			IContextAnalyzer contextAnalzer = new ContextAnalyzer.ContextAnalyzer();

			janCCompiler = new JanCCompiler(contextAnalyzer: contextAnalzer);

			Assert.AreEqual(contextAnalzer, janCCompiler.ContextAnalyzer);
		}

		#endregion

		#region ReadSource Tests

		[TestMethod()]
		[DataRow("test1.jc")]
		public void ReadSource_FileinfoTest(string file) {
			FileInfo fileInfo = new FileInfo(path + file);

			janCCompiler = janCCompiler.ReadSource(fileInfo);

			Assert.AreEqual(fileInfo, janCCompiler.SourceFile);
			Assert.AreEqual(File.ReadAllText(fileInfo.FullName), janCCompiler.SourceString);
		}
		[TestMethod()]
		[DataRow("test1.jc")]
		public void ReadSource_StringTest(string source) {
			janCCompiler = janCCompiler.ReadSource(source);

			Assert.AreEqual(source, janCCompiler.SourceString);
		}

		#endregion

		#region GenerateCST Tests

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateCST_SourceStringNullTest() {
			janCCompiler.GenerateCST();
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateCST_CSTGeneratorNullTest() {
			JanCCompiler janCCompiler = new JanCCompiler();
			janCCompiler.SourceString = "";
			janCCompiler.CSTGenerator = null;

			janCCompiler.GenerateCST();
		}
		[TestMethod()]
		public void GenerateCST_GeneratesCSTTest() {
			janCCompiler = janCCompiler.ReadSource("int a = 5").GenerateCST();

			Assert.IsNotNull(janCCompiler.CSTTree);
			Assert.IsInstanceOfType(janCCompiler.CSTTree, typeof(JanCParser.CompileUnitContext));
		}
		[TestMethod()]
		[ExpectedException(typeof(LanguageException))]
		public void GenerateCST_ThrowsIfParserErrorsTest() {
			janCCompiler.ReadSource("int a 5").GenerateCST();
		}

		#endregion

		#region Generate AST Tests

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateAST_CSTTreeNullTest() {
			janCCompiler.GenerateAST();
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateAST_ASTGeneratorNullTest() {
			JanCCompiler janCCompiler = new JanCCompiler();
			janCCompiler.CSTTree = new ParserRuleContext();
			janCCompiler.ASTGenerator = null;

			janCCompiler.GenerateAST();
		}
		[TestMethod()]
		public void GenerateAST_GeneratesASTTest() {
			janCCompiler.ReadSource("int a = 5").GenerateCST();

			janCCompiler = janCCompiler.GenerateAST();

			Assert.IsNotNull(janCCompiler.ASTTree);
			Assert.IsInstanceOfType(janCCompiler.ASTTree, typeof(GlobalScopeNode));
		}

		#endregion

		#region DecorateAST Tests

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecorateAST_ASTTreeNullTest() {
			janCCompiler.DecorateAST();
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecorateAST_ContextAnalyzerNullTest() {
			JanCCompiler janCCompiler = new JanCCompiler();
			janCCompiler.ASTTree = CommonAST.Root();
			janCCompiler.ContextAnalyzer = null;

			janCCompiler.DecorateAST();
		}
		[TestMethod()]
		[TestCategory("SlowTest")]
		public void DecorateAST_DecorateASTTest() {
			janCCompiler.ReadSource("int a = 5").GenerateCST().GenerateAST();

			janCCompiler = janCCompiler.DecorateAST();
		}
		[TestMethod()]
		[ExpectedException(typeof(LanguageException))]
		public void DecorateAST_ThrowsIfSemanticErrorsTest() {
			janCCompiler.ReadSource("int a = a").GenerateCST().GenerateAST();

			janCCompiler = janCCompiler.DecorateAST();
		}

		#endregion

		#region GenerateCode Tests

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateCode_ASTTreeNullTest() {
			janCCompiler.GenerateCode();
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateCode_ContextAnalyzerNullTest() {
			JanCCompiler janCCompiler = new JanCCompiler();
			janCCompiler.ASTTree = CommonAST.Root();
			janCCompiler.CodeGenerator = null;

			janCCompiler.GenerateCode();
		}
		[TestMethod()]
		[TestCategory("SlowTest")]
		public void GenerateCode_GenerateCodeTest() {
			janCCompiler.ReadSource("int a").GenerateCST().GenerateAST().DecorateAST();

			janCCompiler = janCCompiler.GenerateCode();

			Assert.IsNotNull(janCCompiler.CompiledCCode);
		}

		#endregion

		#region GetErrorAndWarningSummary Tests

		[TestMethod()]
		public void GetErrorAndWarningSummary_GiveNoErrorsOrWarningsIfNoErrorsOrWarningsTest() {
			string summary = janCCompiler.GetErrorAndWarningSummary();

			Assert.AreEqual($"No errors{Environment.NewLine}No warnings", summary);
		}
		[TestMethod()]
		public void GetErrorAndWarningSummary_ErrorsIfErrorsTest() {
			janCCompiler.Errors.Add(new LanguageException("test"));

			string summary = janCCompiler.GetErrorAndWarningSummary();

			Assert.AreEqual($"--- Errors ---{Environment.NewLine}Exceptions.Exceptions.LanguageException: test{Environment.NewLine}{Environment.NewLine}No warnings", summary);
		}

		#endregion

		#region GetErrorSummary Tests

		[TestMethod()]
		public void GetErrorSummary_GiveNoErrorsOrWarningsIfNoErrorsOrWarningsTest() {
			string summary = janCCompiler.GetErrorSummary();

			Assert.AreEqual($"No errors", summary);
		}
		[TestMethod()]
		public void GetErrorSummary_ErrorsIfErrorsTest() {
			janCCompiler.Errors.Add(new LanguageException("test"));

			string summary = janCCompiler.GetErrorSummary();

			Assert.AreEqual($"--- Errors ---{Environment.NewLine}Exceptions.Exceptions.LanguageException: test{Environment.NewLine}", summary);
		}

		#endregion

		#region GetWarningsSummary Tests

		[TestMethod()]
		public void GetWarningsSummary_GiveNoErrorsOrWarningsIfNoErrorsOrWarningsTest() {
			string summary = janCCompiler.GetWarningsSummary();

			Assert.AreEqual($"No warnings", summary);
		}

		#endregion

		#region ThrowIfState Tests

		[TestMethod()]
		public void ThrowIfState_HadWarningsState() {
			JanCCompiler janCCompiler = new JanCCompiler();
			janCCompiler.Warnings.Add(new LanguageException("test"));

			janCCompiler.ThrowIfState();

			Assert.AreEqual(IJanCCompiler.States.FinishedWithWarnings, janCCompiler.State);
		}

		[TestMethod()]
		[ExpectedException(typeof(LanguageException))]
		public void ThrowIfState_HadWarningsAndDontAllowWarningsState() {
			JanCCompiler janCCompiler = new JanCCompiler();
			janCCompiler.Warnings.Add(new LanguageException("test"));
			janCCompiler.AllowWarnings = false;

			janCCompiler.ThrowIfState();
		}

		#endregion
	}
}
