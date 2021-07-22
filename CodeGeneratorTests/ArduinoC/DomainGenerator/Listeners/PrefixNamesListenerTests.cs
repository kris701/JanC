
using Nodes;
using CodeGenerator.ArduinoC.DomainGenerator;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tables;
using Tools;
using System;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass]
	public class PrefixNamesListenerTests {
		#region Test Setup

		private readonly string expectedPrefix = "JanC_";

		private PrefixNamesListener prefixNamesListener;

		[TestInitialize()]
		public void Setup() {
			prefixNamesListener = new PrefixNamesListener();
		}

		#endregion

		#region Enter(ITypedDecl decl)
		[TestMethod]
		[DataRow("func1")]
		[DataRow("setup")]
		[DataRow("loop")]
		public void RenamesFunctions(string name) {
			FuncDeclNode node = Int.Function(name);

			prefixNamesListener.Enter(node);

			Assert.AreEqual($"{expectedPrefix}{name}", node.Name);
		}

		[TestMethod]
		[DataRow("vardecl")]
		[DataRow("loop")]
		public void RenamesVarDecl(string name) {
			VarDeclNode node = Int.VarDecl(name);

			prefixNamesListener.Enter(node);

			Assert.AreEqual($"{expectedPrefix}{name}", node.Name);
		}

		[TestMethod]
		[DataRow("structname")]
		[DataRow("struct")]
		public void RenamesStructDecl(string name) {
			StructDeclNode node = StructDecl(name);

			prefixNamesListener.Enter(node);

			Assert.AreEqual($"{expectedPrefix}{name}", node.Name);
		}

		[TestMethod]
		[DataRow("modulename")]
		[DataRow("module")]
		public void RenamesModuleDecl(string name) {
			ModuleDeclNode node = Module(name);

			prefixNamesListener.Enter(node);

			Assert.AreEqual($"{expectedPrefix}{name}", node.Name);
		}

		#endregion
	}
}
