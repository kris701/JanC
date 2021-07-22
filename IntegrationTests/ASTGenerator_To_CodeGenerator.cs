using ASTGenerator;
using CodeGenerator;
using CodeGenerator.ArduinoC;
using CodeGenerator.ArduinoC.Nodes;
using ContextAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests {
	[TestClass]
	public class ASTGenerator_To_CodeGenerator {
		[TestMethod]
		public void CanGenerateSimpleDomain() {
			// ARRANGE
			IASTBuilder astBuilder = new ASTBuilder().ReadSource(new JanCParser.CompileUnitContext(null ,0)).BuildAST();
			ICodeGenerator codeGenerator = new ArduinoCGenerator();

			// ACT
			codeGenerator = codeGenerator.ReadSource(astBuilder.ASTTree).SetPhases().GenerateDomain();

			// ASSERT
			Assert.IsInstanceOfType(codeGenerator.GeneratedDomain, typeof(GlobalScopeNode));
			Assert.IsInstanceOfType(((GlobalScopeNode)codeGenerator.GeneratedDomain).Children[0], typeof(LibraryNode));
			Assert.IsInstanceOfType(((GlobalScopeNode)codeGenerator.GeneratedDomain).Children[1], typeof(FunctionForwardDeclaration));
			Assert.IsInstanceOfType(((GlobalScopeNode)codeGenerator.GeneratedDomain).Children[2], typeof(FunctionForwardDeclaration));
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfASTIsNull() {
			// ARRANGE
			IASTBuilder astBuilder = new ASTBuilder();
			ICodeGenerator codeGenerator = new ArduinoCGenerator();

			// ACT
			codeGenerator.ReadSource(astBuilder.ASTTree);
		}
	}
}
