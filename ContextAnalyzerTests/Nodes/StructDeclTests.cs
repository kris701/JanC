using ContextAnalyzer;
using ContextAnalyzer.SemanticErrors;
using Exceptions.Syntax;
using Exceptions.Syntax.SemanticErrors;
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
	public class StructDeclTests : BaseRulesTests {

		public StructDeclNode structDecl;
		public StructLiteralNode structLiteral;

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			structDecl = StructDecl("my_struct");
			structLiteral = StructLiteral(structDecl.Type);
		}

		[TestMethod]
		public void StructCanBeUsedBeforeDeclaration() {
			global
				.With(structLiteral)
				.With(structDecl);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void StructCanBeUsedAfterDeclaration() {
			global
				.With(structDecl)
				.With(structLiteral);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredType))]
		public void UndeclaredStructCannotBeUsed() {
			global
				.With(structLiteral);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void StructCanContainOtherStruct() {
			var otherStruct = StructDecl("otherStruct").With(Int.VarDecl("x"));
			structDecl.With(otherStruct.Type.VarDecl("other"));
			global
				.With(structDecl)
				.With(otherStruct);

			ThrowErrorsIfAny();
		}


		[TestMethod]
		public void StructCanBeIndirectlyRecursive() {
			structDecl.With(Ref(structDecl.Type).VarDecl("selfRef"));
			global.With(structDecl);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void StructCanBeIndirectlyMutuallyRecursive() {
			var otherStruct = StructDecl("otherStruct").With(Ref(structDecl.Type).VarDecl("ref"));
			structDecl.With(Ref(otherStruct.Type).VarDecl("otherRef"));
			global
				.With(structDecl)
				.With(otherStruct);

			ThrowErrorsIfAny();
		}


		[TestMethod]
		[ExpectedException(typeof(StructCannotBeRecursive))]
		public void StructCannotBeDirectlyRecursive() {
			structDecl.With(structDecl.Type.VarDecl("selfRef"));
			global.With(structDecl);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(StructsCannotBeMutuallyRecursive))]
		public void StructCannotBeDirectlyMutuallyRecursive() {
			var otherStruct = StructDecl("otherStruct").With(structDecl.Type.VarDecl("ref"));
			structDecl.With(otherStruct.Type.VarDecl("otherRef"));
			global
				.With(structDecl)
				.With(otherStruct);

			ThrowErrorsIfAny();
		}

	}
}
