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

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class ForStatementTests : BaseRulesTests {

		[TestMethod]
		public void InfiniteEmptyLoopWorks() {
			var forStmt = ForStmt(null, null, null, Block());
			global.With(forStmt);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void StandardCounterWorks() {
			var i = Int.VarDecl("i");
			var condition = Identifier(i).UEquals(Literal(10));
			var update = Identifier(i).PlusAssign(Literal(1));
			var forStmt = ForStmt(i, condition, update, Block());
			global.With(forStmt);

			ThrowErrorsIfAny();
		}

	}
}
