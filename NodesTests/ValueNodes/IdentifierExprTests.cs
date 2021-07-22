using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodesTests.ValueNodes {
	[TestClass]
	public class IdentifierExprTests {
		[TestMethod]
		public void LinkNameTo_TakesNameRefOfDeclaration() { // Awkward test, since the IdentifierExpr.NameRef is totally private.
			var iExpr = new IdentifierExpr(null, "something", null);
			var vDeclNode = new VarDeclNode(null, "something else", null, null, false);

			iExpr.LinkNameTo(vDeclNode);

			Assert.IsTrue(iExpr.Name.Equals(vDeclNode.Name));
			vDeclNode.Name = "something third"; // Both values should change here, and thereby still be equivalent.
			Assert.IsTrue(iExpr.Name.Equals(vDeclNode.Name));
		}

		[TestMethod]
		public void LocalRename_PerformsRenaming() {
			var iExpr = new IdentifierExpr(null, "something", null);

			string expected = "something else";
			iExpr.LocalRename(expected);
			string actual = iExpr.Name;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void LocalRename_Detaches() {
			var iExpr = new IdentifierExpr(null, "something", null);
			var vDeclNode = new VarDeclNode(null, "something else", null, null, false);

			iExpr.LinkNameTo(vDeclNode);
			iExpr.LocalRename("Something different!");

			Assert.IsFalse(iExpr.Name.Equals(vDeclNode.Name));
		}
	}
}
