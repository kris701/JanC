using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodes;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.DecoratedAST;
using static Nodes.ASTHelpers.CommonAST;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass()]
	public class ModuleDeclarationRenamerListenerTests {
		#region Test Setup

		private ModuleDeclarationRenamerListener moduleDeclarationRenamerListener;

		[TestInitialize()]
		public void Setup() {
			moduleDeclarationRenamerListener = new ModuleDeclarationRenamerListener();
		}

		#endregion

		[TestMethod()]
		[DataRow("a", "b", "c")]
		[DataRow("morename", "name", "ababab")]
		public void RenamesAllChildrenWithPrefix(string varName, string funcName, string innerModuleName) {
			VarDeclNode varDeclNode = Int.VarDecl(varName);
			FuncDeclNode funcDeclNode = Float.Function(funcName);
			ModuleDeclNode innerModuleDeclNode = Module(innerModuleName);
			ModuleDeclNode moduleDeclNode = Module("testname")
				.With(varDeclNode)
				.With(funcDeclNode)
				.With(innerModuleDeclNode);

			moduleDeclarationRenamerListener.Enter(moduleDeclNode);

			Assert.AreEqual($"{moduleDeclNode.Name}_{varName}", varDeclNode.Name);
			Assert.AreEqual($"{moduleDeclNode.Name}_{funcName}", funcDeclNode.Name);
			Assert.AreEqual($"{moduleDeclNode.Name}_{innerModuleName}", innerModuleDeclNode.Name);
		}
	}
}
