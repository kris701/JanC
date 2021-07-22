using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSTGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Antlr4.Runtime;
using Exceptions.ErrorListeners;
using Tools;

namespace CSTGenerator.Tests {
	[TestClass()]
	public class CSTBuilderTests {
		#region Test Setup
		private string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/TestFiles/";

		private ICSTBuilder cSTBuilder;

		[TestInitialize()]
		public void Setup() {
			cSTBuilder = new CSTBuilder();
		}

		#endregion

		#region Constructor Tests
		[TestMethod]
		public void Constructor_CreatesErrorListeners() {
			Assert.IsNotNull(cSTBuilder.ParserErrorListener);
			Assert.IsNotNull(cSTBuilder.LexerErrorListener);
		}
		[TestMethod]
		public void Constructor_SetLexer() {
			LexerErrorListener lexerErrorListener = new LexerErrorListener();

			cSTBuilder = new CSTBuilder(lexerErrorListener: lexerErrorListener);

			Assert.AreEqual(lexerErrorListener, cSTBuilder.LexerErrorListener);
		}
		[TestMethod]
		public void Constructor_SetParser() {
			ParserErrorListener parserErrorListener = new ParserErrorListener();

			cSTBuilder = new CSTBuilder(parserErrorListener: parserErrorListener);

			Assert.AreEqual(parserErrorListener, cSTBuilder.ParserErrorListener);
		}
		#endregion

		#region ReadSource Tests
		[TestMethod]
		[DataRow("test2.jc")]
		[DataRow("test4.jc")]
		public void ReadSource_FileinfoTest(string testFile) {
			FileInfo fileInfo = new FileInfo(path + testFile);

			cSTBuilder = cSTBuilder.ReadSource(fileInfo);

			Assert.AreEqual(fileInfo, cSTBuilder.SourceFile);
		}
		[TestMethod]
		[DataRow("somesourcetext")]
		public void ReadSource_StringTest(string input) {
			cSTBuilder = cSTBuilder.ReadSource(input);

			Assert.AreEqual(input, cSTBuilder.SourceString);
		}
		#endregion

		#region BuildCST Tests
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildCST_ThrowsIfSourceStringIsNullTest() {
			cSTBuilder.BuildCST();
		}
		[TestMethod]
		[TestCategory("SlowTest")]
		public void BuildCST_OutputBecomesNullIfErrorsTest1() {
			cSTBuilder = cSTBuilder.ReadSource("int a = {}").BuildCST();

			Assert.AreEqual(1, cSTBuilder.ParserErrorListener.Errors.Count);
			Assert.IsNull(cSTBuilder.CSTTree);
		}
		[TestMethod]
		public void BuildCST_OutputBecomesNullIfErrorsTest2() {
			cSTBuilder = cSTBuilder.ReadSource("str a = \"a").BuildCST();

			Assert.AreEqual(1, cSTBuilder.LexerErrorListener.Errors.Count);
			Assert.IsNull(cSTBuilder.CSTTree);
		}
		#endregion
	}
}
