using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using Nodes.ASTHelpers;
using ContextAnalyzer.Nodes;
using ContextAnalyzer.SemanticErrors;
using Nodes;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class RefTests : BaseRulesTests {

		// Literal

		[TestMethod]
		[ExpectedException(typeof(ExpectedLValue))]
		public void CannotRefLiteral() {
			global.With(Ref(Literal(1)));
			ThrowErrorsIfAny();
		}

		// Variables

		[TestMethod]
		public void CanRefVariable() {
			var variable = Int.VarDecl("x");
			var access = Ref(Identifier(variable));
			global.With(variable)
			.With(access);

			ThrowErrorsIfAny();
			Assert.AreEqual(Ref(Int), access.Type);
		}

		[TestMethod]
		public void CanRefRefVariable() {
			var variable = Int.VarDecl("x");
			var referenceVariable = Ref(Int).VarDecl("xRef");
			var access = Ref(Identifier(referenceVariable));
			global.With(variable)
			.With(referenceVariable)
			.With(access);

			ThrowErrorsIfAny();
			Assert.AreEqual(Ref(Ref(Int)), access.Type);
		}

		[TestMethod]
		public void CanRefConstVariable() {
			var variable = Int.ConstVarDecl("x");
			var access = Ref(Identifier(variable));
			global.With(variable)
			.With(access);

			ThrowErrorsIfAny();
			Assert.AreEqual(Ref(Int), access.Type);
		}

		[TestMethod]
		public void CanRefStructInstance() {
			var @struct = StructDecl("Point").With(Int.VarDecl("x"));
			global.With(@struct)
			.With(Ref(TypeIdentifier(@struct)).VarDecl("point"));

			ThrowErrorsIfAny();
		}

		// Binary expressions

		[TestMethod]
		public void CanAccessReffedStructInstance() {
			var @struct = StructDecl("Point").With(Int.VarDecl("x"));
			var variable = TypeIdentifier(@struct).VarDecl("point");
			var access = Ref(Identifier(variable)).UAccess("x");
			global.With(@struct)
			.With(variable)
			.With(access);

			ThrowErrorsIfAny();
			Assert.AreEqual(Int, access.Type);
		}

		[TestMethod]
		public void CanRefStructMember() {
			var @struct = StructDecl("Point").With(Int.VarDecl("x"));
			var variable = TypeIdentifier(@struct).VarDecl("point");
			global.With(@struct)
			.With(variable)
			.With(Ref(Identifier(variable).UAccess("x")));
			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanAddReffedInts() {
			var a = Int.VarDecl("a");
			var b = Int.VarDecl("b");
			var operation = Ref(Identifier(a)).UPlus(Ref(Identifier(b)));
			global.With(a)
			.With(b)
			.With(operation);

			ThrowErrorsIfAny();
			Assert.AreEqual(Int, operation.Type);
		}

		[TestMethod]
		public void CanAddMixedRefInts() {
			var a = Int.VarDecl("a");
			var b = Int.VarDecl("b");
			var operationA = Ref(Identifier(a)).UPlus(Identifier(b));
			var operationB = Identifier(a).UPlus(Ref(Identifier(b)));
			global.With(a)
			.With(b)
			.With(operationA)
			.With(operationB);

			ThrowErrorsIfAny();
			Assert.AreEqual(Int, operationA.Type);
			Assert.AreEqual(Int, operationB.Type);
		}

		[TestMethod]
		public void CanAssignIntToRefInt() {
			var variable = Int.VarDecl("a");
			var refVariable = Ref(Int).VarDecl("aRef", Identifier(variable));
			global.With(variable).With(refVariable);

			ThrowErrorsIfAny();
		}

		// Types

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void CannotRefFunction() {
			var function = Int.Function("function");
			global.With(function)
			.With(Ref(Identifier(function)));
			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void CannotRefModule() {
			var module = Module("Controller");
			global.With(module)
			.With(Ref(Identifier(module)));
			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanRefTypePrimitive() {
			global.With(Ref(Int).VarDecl("x"));
			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanRefTypeStruct() {
			var @struct = StructDecl("Point").With(Int.VarDecl("x"));
			global.With(@struct)
			.With(Ref(@struct.Type).VarDecl("x"));
			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void CannotRefTypeModule() {
			var module = Module("Controller");
			global.With(module)
			.With(new RefTypeNode(null, new UserDefinedTypeNode(null, "Controller")).VarDecl("x"));
			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void CannotRefTypeFunction() {
			var function = Int.Function("get_value");
			global.With(function)
			.With(new RefTypeNode(null, new UserDefinedTypeNode(null, "get_value")).VarDecl("x"));
			ThrowErrorsIfAny();
		}

	}
}
