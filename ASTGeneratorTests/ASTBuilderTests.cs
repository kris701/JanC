using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASTGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Tools;
using Antlr4.Runtime;
using CSTGenerator;
using Nodes;
using Exceptions.Syntax;

namespace ASTGenerator.Tests {
	[TestClass()]
	public class ASTBuilderTests {
		private readonly string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/TestFiles/";

		#region ReadSource Tests
		[TestMethod()]
		public void ReadSource_Test() {
			// ARRANGE
			IASTBuilder aSTBuilder = new ASTBuilder();

			JanCParser.CompileUnitContext compileUnitContext = new JanCParser.CompileUnitContext(null, 0);

			// ACT
			aSTBuilder = aSTBuilder.ReadSource(compileUnitContext);

			// ASSERT
			Assert.AreEqual(compileUnitContext, aSTBuilder.SourceCSTTree);
		}
		#endregion

		#region BuildAST Tests 
		[TestMethod()]
		public void BuildAST_GivesASTOfCompleteUnit() {
			// ARRANGE
			IASTBuilder aSTBuilder = new ASTBuilder();

			JanCParser.CompileUnitContext compileUnitContext = new JanCParser.CompileUnitContext(null, 0);
			aSTBuilder = aSTBuilder.ReadSource(compileUnitContext);

			// ACT
			aSTBuilder = aSTBuilder.BuildAST();

			// ASSERT
			Assert.IsInstanceOfType(aSTBuilder.ASTTree, typeof(GlobalScopeNode));
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildAST_ThrowsIfNoCST() {
			// ARRANGE
			IASTBuilder aSTBuilder = new ASTBuilder();

			// ACT
			aSTBuilder.BuildAST();
		}
		#endregion


	}
}
