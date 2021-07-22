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
	public class VoidTests : BaseRulesTests {

		[TestMethod]
		public void CanHaveVoidFunction() {
			global.With(TypeNode.Void.Function("function"));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidVoidUsage))]
		public void CannotHaveVoidVariable() {
			global.With(TypeNode.Void.VarDecl("variable"));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidVoidUsage))]
		public void CannotHaveRefVoidVariable() {
			global.With(Ref(TypeNode.Void).VarDecl("ref_variable"));

			ThrowErrorsIfAny();
		}

	}
}
