using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContextAnalyzer.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContextAnalyzer.Tables;
using Nodes;
using Exceptions.Syntax.SemanticErrors;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.ASTHelpers.CommonAST;
using Nodes.ASTHelpers;

namespace ContextAnalyzer.Listener.Tests {
	[TestClass()]
	public class StructRecursionListenerTests {
		#region Test Setup
		private ContextSymbolTable contextSymbolTable;
		private StructRecursionListener structRecursionListener;
		private StructDeclNode structDeclNode;

		[TestInitialize]
		public virtual void Setup() {
			contextSymbolTable = new ContextSymbolTable();
			structRecursionListener = new StructRecursionListener(contextSymbolTable);
			structDeclNode = StructDecl("test");
		}
		#endregion

		#region Constructor Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ThrowsIfConstructorItemsNull() {
			new StructRecursionListener(null);
		}
		#endregion

		#region void Leave(StructDeclNode @struct)
		[TestMethod()]
		public void Leave_NormalStructPassesTest() {
			contextSymbolTable.Record(structDeclNode);

			structRecursionListener.Leave(structDeclNode);
		}
		[TestMethod()]
		public void Leave_RefStructPassesTest() {
			 structDeclNode
				.With(Ref(structDeclNode.Type).VarDecl("x", StructLiteral("test")));
			contextSymbolTable.Record(structDeclNode);

			structRecursionListener.Leave(structDeclNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(StructCannotBeRecursive))]
		public void Leave_ThrowsIfRecursiveStructTest() {
			structDeclNode
				.With(StructVarDecl("test", "a"));
			contextSymbolTable.Record(structDeclNode);

			structRecursionListener.Leave(structDeclNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(StructsCannotBeMutuallyRecursive))]
		public void Leave_ThrowsIfMutuallyRecursiveStructTest() {
			StructDeclNode structDeclNodea = StructDecl("test_a").With(StructVarDecl("test_b", "a"));
			StructDeclNode structDeclNodeb = StructDecl("test_b").With(StructVarDecl("test_a", "a"));

			contextSymbolTable.Record(structDeclNodea);
			contextSymbolTable.Record(structDeclNodeb);

			structRecursionListener.Leave(structDeclNodea);
			structRecursionListener.Leave(structDeclNodeb);
		}
		#endregion
	}
}
