using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class ReturnTests : BaseRulesTests {
		private ReturnStmtNode returnStmt;
		private FuncDeclNode function;

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			returnStmt = new ReturnStmtNode(null, null);
			function = new FuncDeclNode(null, "testFunc", TypeNode.Void, new List<VarDeclNode>(), returnStmt);
		}

		public static IEnumerable<object[]> CorrectReturnTypeData => new List<object[]>() {
			new object[] { Int, new IntLiteralNode(null, 5) },
			new object[] { Float, new FloatLiteralNode(null, 5.64) },
			new object[] { TypeNode.String, new StringLiteralNode(null, "This is a string for a test") },
		};
		[TestMethod]
		[DynamicData(nameof(CorrectReturnTypeData))]
		public void WorksWhenReturnTypeIsCorrect(ITypeLiteral funcType, IExpr returnVal) {
			returnStmt.Value = returnVal;
			function = new FuncDeclNode(null, "testFunc", funcType, new List<VarDeclNode>(), returnStmt);
			global
				.With(function);

			ThrowErrorsIfAny();
		}

		public static IEnumerable<object[]> IncorrectReturnTypeData => new List<object[]>() {
			new object[] { Int, new FloatLiteralNode(null, 5.64)  },
			new object[] { Float, new StringLiteralNode(null, "This is a string for a test") },
			new object[] { TypeNode.String, new IntLiteralNode(null, 5) },
		};
		[TestMethod]
		[ExpectedException(typeof(ReturnTypeMismatch))]
		[DynamicData(nameof(IncorrectReturnTypeData))]
		public void ThrowsWhenReturnTypeIsIncorrect(ITypeLiteral funcType, IExpr returnVal) {
			returnStmt.Value = returnVal;
			function = new FuncDeclNode(null, "testFunc", funcType, new List<VarDeclNode>(), returnStmt);
			global
				.With(function);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void VoidReturnTypeWorks() {
			global
				.With(function);

			ThrowErrorsIfAny();
		}
	}
}
