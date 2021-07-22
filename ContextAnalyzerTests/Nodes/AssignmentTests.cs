using ContextAnalyzer.SemanticErrors;
using Exceptions.Syntax.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.ASTHelpers.CommonAST;
using Nodes.ASTHelpers;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class AssignmentTests : BaseRulesTests {

		[TestMethod]
		public void CanAssignToIntIdentifier() {
			global
				.With(Int.VarDecl("value"))
					.With(Identifier("value")
						.Assign(Literal(1)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanAssignToStringIdentifier() {
			global
				.With(TypeNode.String.VarDecl("value"))
					.With(Identifier("value")
						.Assign(Literal("test")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanAssignToStructMemberAccess() {
			var @struct = StructDecl("Point")
					.With(Int.VarDecl("x"));
			global
				.With(@struct)
				.With(@struct.Type.VarDecl("point"))
				.With(UAccess("point", "x").Assign(Literal(1)));

			ThrowErrorsIfAny();
		}
		
		public static IEnumerable<object[]> CannotAssignToValuesWithoutLocation_Data
			=> new List<object[]>{
						new object[]{ Literal(1) },
						new object[]{ Literal(1.0) },
						new object[]{ Literal("test") },
						new object[]{ Literal(2).UPlus(Literal(2)) },
			};
		[TestMethod]
		[ExpectedException(typeof(ExpectedLValueOnLeftSideOfAssignment))]
		[DynamicData(nameof(CannotAssignToValuesWithoutLocation_Data), DynamicDataSourceType.Property)]
		public void CannotAssignToValuesWithoutLocation(IExpr value) {
			global
				.With(value.Assign(value));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanNumericAssignInt() {
			global
				.With(Int.VarDecl("x"))
				.With(Identifier("x")
					.PlusAssign(Identifier("x")));

			ThrowErrorsIfAny();
		}
		
		[TestMethod]
		public void CanNumericAssignFloat() {
			global 
				.With(Float.VarDecl("x"))
				.With(Identifier("x")
					.PlusAssign(Identifier("x")));

			ThrowErrorsIfAny();
		}
		
		[TestMethod]
		[ExpectedException(typeof(AssignmentOperatorTypeMismatch))]
		public void CannotNumericAssignString() {
			global
				.With(TypeNode.String.VarDecl("x"))
				.With(Identifier("x")
					.PlusAssign(Identifier("x")));

			ThrowErrorsIfAny();
		}
		
		[TestMethod]
		[ExpectedException(typeof(AssignmentOperatorTypeMismatch))]
		public void CannotNumericAssignStruct() {
			var @struct = StructDecl("Point")
					.With(Int.VarDecl("x"));
			global
				.With(@struct)
				.With(@struct.Type.VarDecl("point"))
				.With(Identifier("point")
					.PlusAssign(StructLiteral("Point")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(CannotAssignToConst))]
		public void CannotAssignToConstInt() {
			global
				.With(Int.ConstVarDecl("x"))
				.With(Identifier("x")
					.Assign(Literal(1)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(CannotAssignToConst))]
		public void CannotAssignToConstStruct() {
			var @struct = StructDecl("Point")
					.With(Int.VarDecl("x"));
			global
				.With(@struct)
				.With(@struct.Type.ConstVarDecl("point"))
				.With(Identifier("point")
					.Assign(StructLiteral("Point")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(CannotAssignToConst))]
		public void CannotAssignToConstStructMember() {
			var @struct = StructDecl("Point")
					.With(Int.VarDecl("x"));
			global
				.With(@struct)
				.With(@struct.Type.ConstVarDecl("point"))
				.With(UAccess("point", "x").Assign(Literal(5)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanAssignRefToSubtype() {
			global
				.With(Int.VarDecl("x"))
				.With(Ref(Int).VarDecl("y", Identifier("x")))
				.With(Identifier("x")
					.Assign(Identifier("y")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanAssignRefToRef() {
			global
				.With(Ref(Int).VarDecl("x"))
				.With(Ref(Int).VarDecl("y"))
				.With(Identifier("x")
					.Assign(Identifier("y")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(AssignmentTypeMismatch))]
		public void CannotAssignRefToSubtypeMismatch() {
			global
				.With(Int.VarDecl("x"))
				.With(Ref(TypeNode.String).VarDecl("y"))
				.With(Identifier("x")
					.Assign(Identifier("y")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(AssignmentTypeMismatch))]
		public void CannotAssignSubtypeToRefMismatch() {
			global
				.With(Ref(TypeNode.String).VarDecl("x"))
				.With(Int.VarDecl("y"))
				.With(Identifier("x")
					.Assign(Identifier("y")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(AssignmentTypeMismatch))]
		public void CannotAssignRefToRefMismatch() {
			global
				.With(Ref(Int).VarDecl("x"))
				.With(Ref(TypeNode.String).VarDecl("y"))
				.With(Identifier("x").Assign(Identifier("y")));

			ThrowErrorsIfAny();
		}
	}
}
