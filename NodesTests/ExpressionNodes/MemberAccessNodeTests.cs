using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodesTests.ExpressionNodes {
	[TestClass]
	public class MemberAccessNodeTests {
		[TestMethod]
		public void LinkMemberName_TakesNameRefOfDeclaration() { // Checking that names follow, since nameref is private, and we thus can't do referenceEquals.
			var man = new MemberAccessNode(null, null, "a name");
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);
			var vdn = new VarDeclNode(null, "another name", ptn, null,  false);

			man.LinkMemberName(vdn);
			vdn.Name = "a third";

			Assert.AreEqual(man.MemberName, vdn.Name);
		}
	}
}
