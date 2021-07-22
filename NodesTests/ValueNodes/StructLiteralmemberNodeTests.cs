using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodesTests.ValueNodes {
	[TestClass]
	public class StructLiteralmemberNodeTests {
		[TestMethod]
		public void StructLiteralMemberNode_LinkNameTo_TakesTheNameRefOfVarDeclNode() {
			var structLitMemNode = new StructLiteralMemberNode(null, "something", null);
			var vDeclNode = new VarDeclNode(null, "something else", null, null, false);

			structLitMemNode.LinkNameTo(vDeclNode);

			Assert.IsTrue(ReferenceEquals(structLitMemNode.NameRef, vDeclNode.NameRef));
		}

		[TestMethod]
		public void StructLiteralMemberNode_NameGetsLiteral() {
			var structLitMemNode = new StructLiteralMemberNode(null, "something", null);
			string actual = structLitMemNode.Name;
			string expected = "something";

			Assert.AreEqual(actual, expected);
		}

		[TestMethod]
		public void StructLiteralMemberNode_NameSetsNameRefValueWithLiteral() {
			var structLitMemNode = new StructLiteralMemberNode(null, null, null);
			string expected = "something";
			structLitMemNode.Name = expected;
			string actual = structLitMemNode.NameRef.Value;

			Assert.AreEqual(actual, expected);
		}
	}
}
