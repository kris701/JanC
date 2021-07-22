using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using Nodes.ASTHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using ContextAnalyzer.SemanticErrors;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class BlockTests : BaseRulesTests {

		public BlockNode block;
		public VarDeclNode decl1;
		public VarDeclNode decl2;

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			block = Block();
			decl1 = Int.VarDecl("x");
			decl2 = Int.VarDecl("y");
		}

		[TestMethod]
		public void AllowShadowExternalDeclaration() {
			var newDecl1 = decl1.Type.VarDecl(decl1.Name);
			global.With(decl1);
			global.With(block.AppendBody(newDecl1));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void OuterDeclarationCanBeUsedInsideBlock() {
			global.With(decl1);
			global.With(block.AppendBody(Identifier(decl1)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredSymbol))]
		public void InnerDeclarationCannotBeUsedOutsideBlock() {
			global.With(block.AppendBody(decl1));
			global.With(Identifier(decl1));

			ThrowErrorsIfAny();
		}
	}
}
