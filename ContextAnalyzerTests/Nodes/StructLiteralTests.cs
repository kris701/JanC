using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.TypeNode;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class StructLiteralTests : BaseRulesTests {

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			global
				.With(StructDecl("point")
					.With(Int.VarDecl("x"))
					.With(Int.VarDecl("y")));
		}

		[TestMethod]
		public void EmptyLiteralWorks() {
			global
				.With(StructLiteral("point"));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void PartialLiteralWorks() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void PositionalMembersWork() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3))
				.With(StructMember(5)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void NamedMembersWork() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3, "x"))
				.With(StructMember(5, "y")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void MixedMembersWork() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3))
				.With(StructMember(5, "y")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(CannotHaveMoreLiteralMembersThanStructMembers))]
		public void ThrowsOnMoreLiteralMembersThanDeclMembers() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3))
				.With(StructMember(5))
				.With(StructMember(7)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(PositionalMemberAfterNamedMember))]
		public void ThrowsOnNamedMembersBeforePositionalMembers() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3, "x"))
				.With(StructMember(5)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(StructLiteralMemberTypeMismatch))]
		public void ThrowsOnIncorrectMemberType() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3.0)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(StructLiteralMemberNotFound))]
		public void ThrowsOnIncorrectMemberName() {
			global
				.With(StructLiteral("point")
				.With(StructMember(3, "z")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredType))]
		public void ThrowsOnIncorrectStructName() {
			global
				.With(StructLiteral("points"));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredType))]
		public void ThrowsOnExistingButInvalidIdentifier() {
			global
				.With(Int.VarDecl("a", Literal(5)))
				.With(StructLiteral("a"));

			ThrowErrorsIfAny();
		}
	}
}
