using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.TypeNode;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class VariableTests : BaseRulesTests {
		public VarDeclNode firstDecl;
		public VarDeclNode secondDecl;

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			firstDecl = Int.VarDecl("a");
			secondDecl = Int.VarDecl("a");
		}

		[TestMethod]
		[ExpectedException(typeof(SymbolAlreadyDeclared))]
		public void CannotRedeclareVariableInSameScope() {
			global
				.With(firstDecl)
				.With(secondDecl);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanShadowVariableInFuncParams() {
			global
				.With(firstDecl)
				.With(Int.Function("func")
					.AppendBody(secondDecl));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanRedeclareVariableOutsideScope() {
			global
				.With(Int.Function("func")
					.AppendBody(firstDecl))
				.With(secondDecl);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredSymbol))]
		public void CannotUseVariableBeforeDeclaration() {
			IdentifierExpr identifierExpr = Identifier("a");
			global
				.With(identifierExpr)
				.With(firstDecl);

			ThrowErrorsIfAny();
		}
	}
}
