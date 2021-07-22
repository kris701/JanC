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
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;

namespace IntegrationTests {
	[TestClass]
	public class ContextAnalyzer_To_CodeGenerator {
		[TestMethod]
		public void CanGenerateSimpleDomain() {
			// ARRANGE
			IContextAnalyzer contextAnalzer = new ContextAnalyzer.ContextAnalyzer().ReadSource(Root()).SetPhases().SetErrorListener().DecorateAST();
			ICodeGenerator codeGenerator = new ArduinoCGenerator();

			// ACT
			codeGenerator = codeGenerator.ReadSource(contextAnalzer.DecoratedAST).SetPhases().GenerateDomain();

			// ASSERT
			Assert.IsInstanceOfType(codeGenerator.GeneratedDomain, typeof(GlobalScopeNode));
			Assert.IsInstanceOfType(((GlobalScopeNode)codeGenerator.GeneratedDomain).Children[0], typeof(LibraryNode));
			Assert.IsInstanceOfType(((GlobalScopeNode)codeGenerator.GeneratedDomain).Children[1], typeof(FunctionForwardDeclaration));
			Assert.IsInstanceOfType(((GlobalScopeNode)codeGenerator.GeneratedDomain).Children[2], typeof(FunctionForwardDeclaration));
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfAnalyzedASTBecomesNull() {
			// ARRANGE
			IContextAnalyzer contextAnalzer = new ContextAnalyzer.ContextAnalyzer().ReadSource(
				Root().With(TypeNode.Void.Function("a")).With(TypeNode.Void.Function("a"))
				).SetPhases().SetErrorListener().DecorateAST();
			ICodeGenerator codeGenerator = new ArduinoCGenerator();

			// ACT
			codeGenerator.ReadSource(contextAnalzer.DecoratedAST);
		}
	}
}
